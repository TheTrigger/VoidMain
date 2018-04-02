using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineIinterface.IO.InputHandlers;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO
{
    public class ConsoleCommandLineReader : ICommandLineReader
    {
        private readonly IConsole _console;
        private readonly ICommandLineViewProvider _viewProvider;
        private readonly IConsoleInputHandler[] _inputHandlers;
        private readonly ConsoleKeyAsyncReader _keyReader;

        public ConsoleCommandLineReader(IConsole console,
            ICommandLineViewProvider viewSelector, IEnumerable<IConsoleInputHandler> inputHandlers)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _viewProvider = viewSelector ?? throw new ArgumentNullException(nameof(viewSelector));
            if (inputHandlers == null)
            {
                throw new ArgumentNullException(nameof(inputHandlers));
            }
            _inputHandlers = inputHandlers.OrderBy(_ => _.Order).ToArray();
            _keyReader = new ConsoleKeyAsyncReader(_console, 100, 1000);
        }

        public Task<string> ReadLineAsync(CancellationToken token = default(CancellationToken))
        {
            return ReadLineAsync(prompt: null, token: token);
        }

        public Task<string> ReadLineAsync(char? mask, CancellationToken token = default(CancellationToken))
        {
            return ReadLineAsync(prompt: null, mask: mask, token: token);
        }

        public Task<string> ReadLineAsync(ICommandLinePrompt prompt,
            CancellationToken token = default(CancellationToken))
        {
            var viewOptions = CommandLineViewOptions.Normal;
            return ReadLineAsync(prompt, viewOptions, token);
        }

        public Task<string> ReadLineAsync(ICommandLinePrompt prompt, char? mask,
            CancellationToken token = default(CancellationToken))
        {
            var viewOptions = mask.HasValue
                ? CommandLineViewOptions.Masked(mask.Value)
                : CommandLineViewOptions.Hidden;
            return ReadLineAsync(prompt, viewOptions, token);
        }

        private async Task<string> ReadLineAsync(ICommandLinePrompt prompt,
            CommandLineViewOptions viewOptions, CancellationToken token)
        {
            ThrowIfCancellationRequested(token, hadUserInput: false);

            string promptMessage = prompt?.GetMessage();
            if (!String.IsNullOrEmpty(promptMessage))
            {
                _console.Write(promptMessage);
            }

            var lineView = _viewProvider.GetView(viewOptions);
            var lineViewLifecycle = lineView as ICommandLineViewLifecycle;
            var eventArgs = new ConsoleInputEventArgs();

            lineViewLifecycle?.BeginReadingLine();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var key = await _keyReader.ReadKeyAsync(token, intercept: true).ConfigureAwait(false);
                    var keyInfo = key.KeyInfo;
                    bool isNextKeyAvailable = key.IsNextKeyAvailable;

                    eventArgs.LineView = lineView;
                    eventArgs.Input = keyInfo;
                    eventArgs.IsNextKeyAvailable = isNextKeyAvailable;
                    eventArgs.IsHandledHint = false;

                    lineViewLifecycle?.BeginHandlingInput(isNextKeyAvailable);
                    for (int i = 0; i < _inputHandlers.Length; i++)
                    {
                        _inputHandlers[i].Handle(eventArgs);
                    }
                    lineViewLifecycle?.EndHandlingInput(isNextKeyAvailable);

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        _console.WriteLine();
                        return lineView.ToString();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                ThrowIfCancellationRequested(token, hadUserInput: lineView.Length > 0);
            }
            finally
            {
                lineViewLifecycle?.EndReadingLine();
            }

            ThrowIfCancellationRequested(token, hadUserInput: lineView.Length > 0);
            return default(string);
        }

        private void ThrowIfCancellationRequested(CancellationToken token, bool hadUserInput)
        {
            if (token.IsCancellationRequested)
            {
                throw new ReadingLineCanceledException(token, hadUserInput);
            }
        }
    }
}
