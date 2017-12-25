using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleCommandLineReader : ICommandLineReader
    {
        private readonly IConsole _console;
        private readonly ICommandLineViewManager _viewManager;
        private readonly IConsoleInputHandler[] _inputHandlers;
        private readonly ConsoleKeyAsyncReader _keyReader;

        public ConsoleCommandLineReader(IConsole console,
            ICommandLineViewManager viewManager, IEnumerable<IConsoleInputHandler> inputHandlers)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _viewManager = viewManager ?? throw new ArgumentNullException(nameof(viewManager));
            if (inputHandlers == null)
            {
                throw new ArgumentNullException(nameof(inputHandlers));
            }
            _inputHandlers = inputHandlers.OrderBy(_ => _.Order).ToArray();
            _keyReader = new ConsoleKeyAsyncReader(_console, 100, 1000);
        }

        public async Task<string> ReadLineAsync(IPrompt prompt, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            string promptMessage = prompt.GetMessage();
            if (!String.IsNullOrEmpty(promptMessage))
            {
                _console.Write(promptMessage);
            }

            while (!token.IsCancellationRequested)
            {
                var keyInfo = await _keyReader.ReadKeyAsync(token, intercept: true).ConfigureAwait(false);

                _viewManager.StartChanges();
                var lineView = _viewManager.GetView();

                var eventArgs = new ConsoleInputEventArgs();
                eventArgs.LineView = lineView;
                eventArgs.Input = keyInfo;
                eventArgs.IsHandledHint = false;

                for (int i = 0; i < _inputHandlers.Length; i++)
                {
                    _inputHandlers[i].Handle(eventArgs);
                }
                _viewManager.EndChanges();

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    return FinishRead(lineView);
                }
            }

            token.ThrowIfCancellationRequested();
            return default(string);
        }

        private string FinishRead(ICommandLineView lineView)
        {
            string commandLine = lineView.ToString();
            _viewManager.ResetState();
            _console.WriteLine();
            return commandLine;
        }
    }
}
