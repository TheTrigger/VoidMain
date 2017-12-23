using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Builder;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.Hosting;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleInterface : ICommandLineIinterface
    {
        private readonly IConsole _console;
        private readonly ICommandLineReader _commandLineReader;
        private readonly ICommandLineParser _commandLineParser;
        private readonly ConsoleLockingOutput _output;
        private CancellationTokenSource _cliLoopTokenSource;
        private Task _cliLoop;
        public bool IsRunning { get; private set; }

        public ConsoleInterface(IConsole console,
            ICommandLineReader commandLineReader, ICommandLineParser commandLineParser)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _commandLineReader = commandLineReader ?? throw new ArgumentNullException(nameof(commandLineReader));
            _commandLineParser = commandLineParser ?? throw new ArgumentNullException(nameof(commandLineParser));
            _output = new ConsoleLockingOutput(console);
            _cliLoopTokenSource = new CancellationTokenSource();
            _cliLoop = null;
            IsRunning = false;
        }

        public async Task StartAsync(CommandDelegate application, CancellationToken token = default(CancellationToken))
        {
            await Task.Yield();

            token.ThrowIfCancellationRequested();
            if (IsRunning)
            {
                throw new InvalidOperationException("Command-line interface is already started.");
            }
            if (_cliLoopTokenSource.IsCancellationRequested)
            {
                _cliLoopTokenSource.Dispose();
                _cliLoopTokenSource = new CancellationTokenSource();
            }

            token.ThrowIfCancellationRequested();
            _cliLoop = CommandLineLoop(application, _cliLoopTokenSource.Token);
        }

        public async Task StopAsync(CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();
            _cliLoopTokenSource.Cancel();
            await WaitForShutdownAsync(token).ConfigureAwait(false);
        }

        public Task WaitForShutdownAsync(CancellationToken token = default(CancellationToken))
        {
            if (_cliLoop == null)
            {
                return Task.CompletedTask;
            }
            if (!token.CanBeCanceled)
            {
                return _cliLoop;
            }
            var canceled = new TaskCompletionSource<object>();
            token.Register(canceled.SetCanceled);
            return Task.WhenAny(_cliLoop, canceled.Task);
        }

        private async Task CommandLineLoop(CommandDelegate application, CancellationToken loopToken)
        {
            await Task.Yield();

            var consoleTokenSource = new ConsoleCancelEventTokenSource(_console);
            loopToken.Register(consoleTokenSource.Cancel);
            IsRunning = true;

            bool isFirstCancel = true;
            string commandLine = null;

            try
            {
                while (!loopToken.IsCancellationRequested)
                {
                    try
                    {
                        _output.LockForRead();
                        commandLine = await _commandLineReader.ReadLineAsync(consoleTokenSource.Token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        if (loopToken.IsCancellationRequested)
                        {
                            // Do not throw because this is a normal loop ending.
                            break;
                        }
                        consoleTokenSource.Reset();
                        _console.WriteLine();
                        if (isFirstCancel)
                        {
                            isFirstCancel = false;
                            continue;
                        }
                        else
                        {
                            // Exit on double Ctrl+C hotkey.
                            break;
                        }
                    }
                    finally
                    {
                        _output.Unlock();
                    }

                    // Reset flag after succesfull reading.
                    isFirstCancel = true;

                    if (String.IsNullOrEmpty(commandLine))
                    {
                        continue;
                    }

                    try
                    {
                        var context = new Dictionary<string, object>();
                        _commandLineParser.ParseToContext(commandLine, context);
                        context[ContextKey.CommandCancelled] = consoleTokenSource.Token;
                        context[ContextKey.Output] = _output;

                        if (loopToken.IsCancellationRequested)
                        {
                            break;
                        }

                        await application(context).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        if (loopToken.IsCancellationRequested)
                        {
                            // Do not throw because this is a normal loop ending.
                            break;
                        }
                        consoleTokenSource.Reset();
                    }
                    catch (Exception ex)
                    {
                        PriontErrorMessage(ex, verbose: false);
                    }
                }
            }
            finally
            {
                // TODO: Fix hanging.
                consoleTokenSource.Dispose();
                IsRunning = false;
            }
        }

        private void PriontErrorMessage(Exception ex, bool verbose)
        {
            if (verbose)
            {
                _console.WriteLine("Error: " + ex.ToString());
            }
            else
            {
                _console.WriteLine("Error: " + ex.Message);
            }
            _console.WriteLine();
        }
    }
}
