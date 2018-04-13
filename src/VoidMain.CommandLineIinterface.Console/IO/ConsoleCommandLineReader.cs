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
        private readonly IConsoleKeyReader _keyReader;
        private readonly ICommandLineViewProvider _viewProvider;
        private readonly IConsoleInputHandler[] _inputHandlers;

        public ConsoleCommandLineReader(IConsole console, IConsoleKeyReader keyReader,
            ICommandLineViewProvider viewProvider, IEnumerable<IConsoleInputHandler> inputHandlers)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _keyReader = keyReader ?? throw new ArgumentNullException(nameof(keyReader));
            _viewProvider = viewProvider ?? throw new ArgumentNullException(nameof(viewProvider));
            if (inputHandlers == null)
            {
                throw new ArgumentNullException(nameof(inputHandlers));
            }
            _inputHandlers = inputHandlers.OrderBy(_ => _.Order).ToArray();
        }

        public Task<string> ReadLineAsync(CancellationToken token = default(CancellationToken))
        {
            var viewOptions = CommandLineViewOptions.Normal;
            return ReadLineAsync(viewOptions, token);
        }

        public Task<string> ReadLineAsync(char? mask, CancellationToken token = default(CancellationToken))
        {
            var viewOptions = mask.HasValue
                ? CommandLineViewOptions.Masked(mask.Value)
                : CommandLineViewOptions.Hidden;
            return ReadLineAsync(viewOptions, token);
        }

        private async Task<string> ReadLineAsync(CommandLineViewOptions viewOptions, CancellationToken token)
        {
            ThrowIfCancellationRequested(token, hadUserInput: false);

            var lineView = _viewProvider.GetView(viewOptions);
            var lineViewLifecycle = lineView as ICommandLineInputLifecycle;
            var eventArgs = new ConsoleInputEventArgs();

            lineViewLifecycle?.BeforeLineReading();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var key = await _keyReader.ReadKeyAsync(intercept: true, token: token).ConfigureAwait(false);
                    var keyInfo = key.KeyInfo;
                    bool isNextKeyAvailable = key.IsNextKeyAvailable;

                    eventArgs.LineView = lineView;
                    eventArgs.Input = keyInfo;
                    eventArgs.IsNextKeyAvailable = isNextKeyAvailable;
                    eventArgs.IsHandledHint = false;

                    lineViewLifecycle?.BeforeInputHandling(isNextKeyAvailable);
                    for (int i = 0; i < _inputHandlers.Length; i++)
                    {
                        _inputHandlers[i].Handle(eventArgs);
                    }
                    lineViewLifecycle?.AfterInputHandling(isNextKeyAvailable);

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
                lineViewLifecycle?.AfterLineReading();
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
