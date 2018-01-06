using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Builder;
using VoidMain.CommandLineIinterface.Internal;
using VoidMain.CommandLineIinterface.IO;
using VoidMain.CommandLineIinterface.IO.Console.Internal;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleInterface : ICommandLineIinterface
    {
        private readonly IConsole _console;
        private readonly IPrompt _prompt;
        private readonly ICommandLineReader _reader;
        private readonly ICommandLineParser _parser;
        private readonly ConsoleLockingOutput _output;
        private readonly ContextCreationHelper _contextHelper;
        private CancellationTokenSource _cliLoopTokenSource;
        private Task _cliLoop;
        public bool IsRunning { get; private set; }

        public ConsoleInterface(IConsole console, IPrompt prompt,
            ICommandLineReader reader, ICommandLineParser parser)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _prompt = prompt ?? throw new ArgumentNullException(nameof(prompt));
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _output = new ConsoleLockingOutput(console);
            _contextHelper = new ContextCreationHelper();
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
                        commandLine = await _reader.ReadLineAsync(_prompt, consoleTokenSource.Token)
                            .ConfigureAwait(false);
                    }
                    catch (OperationCanceledException ex)
                    {
                        // Do not throw because this is a normal loop ending.
                        if (loopToken.IsCancellationRequested) break;

                        consoleTokenSource.Reset();
                        _console.WriteLine();

                        if (ex is ReadingLineCanceledException rex && rex.HadUserInput)
                        {
                            // Do not count this as a first cancellation if there was a user input
                            // and reset the flag if it was not the first one.
                            isFirstCancel = true;
                            continue;
                        }

                        // Exit on double Ctrl+C hotkey.
                        if (!isFirstCancel) break;

                        isFirstCancel = false;
                        _console.WriteLine("Press Ctrl+C again to close application");
                        continue;
                    }
                    finally
                    {
                        _output.Unlock();
                    }

                    // Reset the flag after a succesfull reading.
                    isFirstCancel = true;

                    if (String.IsNullOrWhiteSpace(commandLine)) continue;

                    try
                    {
                        var parsedCommandLine = _parser.Parse(commandLine, consoleTokenSource.Token);

                        if (parsedCommandLine.HasErrors)
                        {
                            PrintParseErrors(parsedCommandLine);
                            continue;
                        }

                        var context = new Dictionary<string, object>();
                        _contextHelper.UseContext(context)
                            .SetOutput(_output)
                            .SetCancellation(consoleTokenSource.Token)
                            .SetRawCommandLine(commandLine)
                            .SetParsedCommandLine(parsedCommandLine);

                        // End command-line loop.
                        if (loopToken.IsCancellationRequested) break;

                        await application(context).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        // Do not throw because this is a normal loop ending.
                        if (loopToken.IsCancellationRequested) break;

                        consoleTokenSource.Reset();
                        _console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        PrintErrorMessage(ex, verbose: false);
                    }
                }
            }
            finally
            {
                consoleTokenSource.Dispose();
                IsRunning = false;
            }
        }

        private void PrintParseErrors(CommandLineSyntax commandLineSyntax)
        {
            _console.WriteLine(commandLineSyntax.FullSpan);

            var errors = commandLineSyntax.Errors;
            int indexSpace = errors.Count.ToString().Length;

            for (int i = 0; i < errors.Count; i++)
            {
                var error = errors[i];
                var span = error.Span;

                _console.Write(' ', span.Start);
                _console.Write('^', span.IsEmpty ? 1 : span.Length);

                string index = (i + 1).ToString().PadLeft(indexSpace);
                _console.WriteLine(index);
            }

            for (int i = 0; i < errors.Count; i++)
            {
                var error = errors[i];
                string index = (i + 1).ToString().PadLeft(indexSpace);
                _console.WriteLine($"{index}) {error.Message}");
            }

            _console.WriteLine();
        }

        private void PrintErrorMessage(Exception ex, bool verbose)
        {
            string errorMessage = verbose
                ? ex.ToString()
                : ex.Message;

            _console.WriteLine("Error: " + errorMessage);
            _console.WriteLine();
        }
    }
}
