using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Builder;
using VoidMain.CommandLineInterface.Internal;
using VoidMain.CommandLineInterface.IO;
using VoidMain.CommandLineInterface.IO.Console;
using VoidMain.CommandLineInterface.IO.Prompt;
using VoidMain.CommandLineInterface.Parser;
using VoidMain.CommandLineInterface.Parser.Syntax;

namespace VoidMain.CommandLineInterface
{
    public class ConsoleInterface : ICommandLineInterface
    {
        private readonly IConsole _console;
        private readonly IColoredTextWriter _coloredTextWriter;
        private readonly ConsoleOutputLock _outputLock;
        private readonly ILineReader _lineReader;
        private readonly ICommandLineParser _parser;
        private readonly IPromptMessage _prompt;
        private readonly ContextBuilder _contextBuilder;
        private CancellationTokenSource _cliLoopTokenSource;
        private Task _cliLoop;
        public bool IsRunning { get; private set; }

        public ConsoleInterface(
            IConsole console,
            IColoredTextWriter coloredTextWriter,
            ConsoleOutputLock outputLock,
            ILineReader lineReader,
            ICommandLineParser parser,
            IPromptMessage prompt = null)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _coloredTextWriter = coloredTextWriter ?? throw new ArgumentNullException(nameof(coloredTextWriter));
            _outputLock = outputLock ?? throw new ArgumentNullException(nameof(outputLock));
            _lineReader = lineReader ?? throw new ArgumentNullException(nameof(lineReader));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _prompt = prompt;
            _contextBuilder = new ContextBuilder();
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
                        _outputLock.Lock();
                        _prompt?.Write(_coloredTextWriter);
                        commandLine = await _lineReader.ReadLineAsync(consoleTokenSource.Token)
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
                        _outputLock.Unlock();
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

                        var context = _contextBuilder
                            .SetCancelToken(consoleTokenSource.Token)
                            .SetRawCommandLine(commandLine)
                            .SetParsedCommandLine(parsedCommandLine)
                            .Build();

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
            var errors = commandLineSyntax.Errors;

            _console.WriteLine(commandLineSyntax.FullSpan);
            PrintParseErrorsMarkers(errors);

            int indexSpace = errors.Count.ToString().Length;

            for (int i = 0; i < errors.Count; i++)
            {
                var error = errors[i];
                string errorIndex = (i + 1).ToString().PadLeft(indexSpace);
                _console.WriteLine($"{errorIndex}) {error.Message}");
            }

            _console.WriteLine();
        }

        private void PrintParseErrorsMarkers(IReadOnlyList<SyntaxError> errors)
        {
            var errorMarkers = new List<SyntaxError>(errors);
            bool hasMoreMarkers = false;
            int linePos = 0;

            do
            {
                hasMoreMarkers = false;
                linePos = 0;

                for (int i = 0; i < errorMarkers.Count; i++)
                {
                    var marker = errorMarkers[i];
                    if (marker == null)
                    {
                        // Already printed
                        continue;
                    }

                    var span = marker.Span;
                    if (span.Start < linePos)
                    {
                        hasMoreMarkers = true;
                        continue;
                    }

                    int markerOffset = span.Start - linePos;
                    _console.Write(' ', markerOffset);
                    linePos += markerOffset;

                    int markerWidth = span.IsEmpty ? 1 : span.Length;
                    _console.Write('^', markerWidth);
                    linePos += markerWidth;

                    string markerIndex = (i + 1).ToString();
                    _console.Write(markerIndex);
                    linePos += markerIndex.Length;

                    // Mark as printed
                    errorMarkers[i] = null;
                }

                _console.WriteLine();
            } while (hasMoreMarkers);
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
