using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineIinterface.IO.Console.InputHandlers;
using VoidMain.CommandLineIinterface.IO.Console.Internal;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO.Console
{
    public class ConsoleCommandLineReader : ICommandLineReader
    {
        private readonly IConsole _console;
        private readonly ICommandLineViewProvider _viewProvider;
        private readonly IConsoleInputHandler[] _inputHandlers;
        private readonly ConsoleKeyAsyncReader _keyReader;

        public ConsoleCommandLineReader(IConsole console,
            ICommandLineViewProvider viewProvider, IEnumerable<IConsoleInputHandler> inputHandlers)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _viewProvider = viewProvider ?? throw new ArgumentNullException(nameof(viewProvider));
            if (inputHandlers == null)
            {
                throw new ArgumentNullException(nameof(inputHandlers));
            }
            _inputHandlers = inputHandlers.OrderBy(_ => _.Order).ToArray();
            _keyReader = new ConsoleKeyAsyncReader(_console, 100, 1000);
        }

        public Task<string> ReadLineAsync(IPrompt prompt, CancellationToken token)
        {
            var viewOptions = CommandLineViewOptions.Normal;
            return ReadLineAsync(prompt, viewOptions, token);
        }

        public Task<string> ReadLineAsync(IPrompt prompt, char? mask, CancellationToken token)
        {
            var viewOptions = mask.HasValue
                ? CommandLineViewOptions.Masked(mask.Value)
                : CommandLineViewOptions.Hidden;
            return ReadLineAsync(prompt, viewOptions, token);
        }

        private async Task<string> ReadLineAsync(IPrompt prompt,
            CommandLineViewOptions viewOptions, CancellationToken token)
        {
            ThrowIfCancellationRequested(token, hadUserInput: false);

            string promptMessage = prompt.GetMessage();
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
                    var keyInfo = await _keyReader.ReadKeyAsync(token, intercept: true).ConfigureAwait(false);

                    eventArgs.LineView = lineView;
                    eventArgs.Input = keyInfo;
                    eventArgs.IsHandledHint = false;

                    lineViewLifecycle?.BeginHandlingInput();
                    for (int i = 0; i < _inputHandlers.Length; i++)
                    {
                        _inputHandlers[i].Handle(eventArgs);
                    }
                    lineViewLifecycle?.EndHandlingInput();

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
