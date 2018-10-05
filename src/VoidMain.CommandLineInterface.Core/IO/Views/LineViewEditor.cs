using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineInterface.IO.InputHandlers;

namespace VoidMain.CommandLineInterface.IO.Views
{
    public class LineViewEditor : ILineViewEditor
    {
        private readonly IInputKeyReader _keyReader;
        private readonly IInputHandler[] _inputHandlers;

        public LineViewEditor(
            IInputKeyReader keyReader,
            IEnumerable<IInputHandler> inputHandlers)
        {
            _keyReader = keyReader ?? throw new ArgumentNullException(nameof(keyReader));
            if (inputHandlers == null)
            {
                throw new ArgumentNullException(nameof(inputHandlers));
            }
            _inputHandlers = inputHandlers.OrderBy(_ => _.Order).ToArray();
        }

        public async Task Edit(ILineView lineView, CancellationToken token = default)
        {
            if(lineView == null)
            {
                throw new ArgumentNullException(nameof(lineView));
            }

            token.ThrowIfCancellationRequested();

            var inputLifecycle = lineView as ILineViewInputLifecycle;
            var eventArgs = new InputEventArgs();

            inputLifecycle?.BeforeLineReading();
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var keyInfo = await _keyReader.ReadKeyAsync(intercept: true, token: token)
                        .ConfigureAwait(false);

                    eventArgs.Input = keyInfo;
                    eventArgs.IsHandledHint = false;
                    eventArgs.LineView = lineView;

                    inputLifecycle?.BeforeInputHandling(keyInfo.HasMoreInput);
                    for (int i = 0; i < _inputHandlers.Length; i++)
                    {
                        _inputHandlers[i].Handle(eventArgs);
                    }
                    inputLifecycle?.AfterInputHandling(keyInfo.HasMoreInput);

                    if (keyInfo.Key == InputKey.Enter) return;
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
