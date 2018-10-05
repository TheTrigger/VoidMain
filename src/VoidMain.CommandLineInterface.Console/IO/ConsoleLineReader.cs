using System;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineInterface.IO.Console;
using VoidMain.CommandLineInterface.IO.Views;

namespace VoidMain.CommandLineInterface.IO
{
    public class ConsoleLineReader : ILineReader
    {
        private readonly IConsole _console;
        private readonly ILineViewEditor _viewEditor;
        private readonly ILineViewProvider _viewProvider;

        public ConsoleLineReader(
            IConsole console,
            ILineViewEditor viewEditor,
            ILineViewProvider viewProvider)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _viewEditor = viewEditor ?? throw new ArgumentNullException(nameof(viewEditor));
            _viewProvider = viewProvider ?? throw new ArgumentNullException(nameof(viewProvider));
        }

        public Task<string> ReadLineAsync(CancellationToken token = default)
        {
            var viewOptions = LineViewOptions.Normal;
            return ReadLineAsync(viewOptions, token);
        }

        public Task<string> ReadLineAsync(char? mask, CancellationToken token = default)
        {
            var viewOptions = mask.HasValue
                ? LineViewOptions.Masked(mask.Value)
                : LineViewOptions.Hidden;
            return ReadLineAsync(viewOptions, token);
        }

        private async Task<string> ReadLineAsync(LineViewOptions viewOptions, CancellationToken token)
        {
            ThrowIfCancellationRequested(token, hadUserInput: false);

            var lineView = _viewProvider.GetView(viewOptions);
            try
            {
                await _viewEditor.Edit(lineView, token);
                _console.WriteLine();
                return lineView.ToString();
            }
            catch (OperationCanceledException)
            {
                ThrowIfCancellationRequested(token, hadUserInput: lineView.Length > 0);
            }

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
