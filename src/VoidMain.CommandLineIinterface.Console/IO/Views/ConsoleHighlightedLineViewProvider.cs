using System;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleHighlightedLineViewProvider : ILineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly ITextHighlighter<ConsoleTextStyle> _textHighlighter;

        public ConsoleHighlightedLineViewProvider(
            IConsole console, IConsoleCursor cursor,
            ITextHighlighter<ConsoleTextStyle> textHighlighter)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _textHighlighter = textHighlighter ?? throw new ArgumentNullException(nameof(textHighlighter));
        }

        public ILineView GetView(LineViewOptions options)
        {
            switch (options.ViewType)
            {
                case LineViewType.Normal:
                    return new ConsoleHighlightedLineView(_console, _cursor, _textHighlighter);
                case LineViewType.Masked:
                    return new ConsoleMaskedLineView(_console, _cursor, options.MaskSymbol);
                case LineViewType.Hidden:
                    return new InMemoryLineView();
                default:
                    throw new NotSupportedException($"{options.ViewType} view is not supported.");
            }
        }
    }
}
