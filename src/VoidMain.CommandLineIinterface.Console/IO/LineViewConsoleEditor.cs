using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.IO.InputHandlers;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO
{
    public class LineViewConsoleEditor : ILineViewEditor
    {
        private readonly IConsoleKeyReader _keyReader;
        private readonly IConsoleInputHandler[] _inputHandlers;

        public LineViewConsoleEditor(
            IConsoleKeyReader keyReader,
            IEnumerable<IConsoleInputHandler> inputHandlers)
        {
            _keyReader = keyReader ?? throw new ArgumentNullException(nameof(keyReader));
            if (inputHandlers == null)
            {
                throw new ArgumentNullException(nameof(inputHandlers));
            }
            _inputHandlers = inputHandlers.OrderBy(_ => _.Order).ToArray();
        }

        public async Task Edit(ILineView lineView, CancellationToken token = default(CancellationToken))
        {
            if(lineView == null)
            {
                throw new ArgumentNullException(nameof(lineView));
            }

            token.ThrowIfCancellationRequested();

            var inputLifecycle = lineView as ILineViewInputLifecycle;
            var eventArgs = new ConsoleInputEventArgs();

            inputLifecycle?.BeforeLineReading();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var key = await _keyReader.ReadKeyAsync(intercept: true, token: token)
                        .ConfigureAwait(false);
                    var keyInfo = key.KeyInfo;
                    bool isNextKeyAvailable = key.IsNextKeyAvailable;

                    eventArgs.LineView = lineView;
                    eventArgs.Input = keyInfo;
                    eventArgs.IsNextKeyAvailable = isNextKeyAvailable;
                    eventArgs.IsHandledHint = false;

                    inputLifecycle?.BeforeInputHandling(isNextKeyAvailable);
                    for (int i = 0; i < _inputHandlers.Length; i++)
                    {
                        _inputHandlers[i].Handle(eventArgs);
                    }
                    inputLifecycle?.AfterInputHandling(isNextKeyAvailable);

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        return;
                    }
                }
            }
            finally
            {
                inputLifecycle?.AfterLineReading();
            }

            token.ThrowIfCancellationRequested();
        }
    }
}
