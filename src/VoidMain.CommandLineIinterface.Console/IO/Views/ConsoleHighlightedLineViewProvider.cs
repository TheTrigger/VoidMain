using System;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleHighlightedLineViewProvider : ILineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _consoleCursor;
        private readonly IColoredTextWriter _coloredTextWriter;
        private readonly ITextHighlighter<TextStyle> _textHighlighter;

        public ConsoleHighlightedLineViewProvider(
            IConsole console, IConsoleCursor consoleCursor,
            IColoredTextWriter coloredTextWriter,
            ITextHighlighter<TextStyle> textHighlighter)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _consoleCursor = consoleCursor ?? throw new ArgumentNullException(nameof(consoleCursor));
            _coloredTextWriter = coloredTextWriter ?? throw new ArgumentNullException(nameof(coloredTextWriter));
            _textHighlighter = textHighlighter ?? throw new ArgumentNullException(nameof(textHighlighter));
        }

        public ILineView GetView(LineViewOptions options)
        {
            switch (options.ViewType)
            {
                case LineViewType.Normal:
                    return new ConsoleHighlightedLineView(_console, _consoleCursor, _coloredTextWriter, _textHighlighter);
                case LineViewType.Masked:
                    return new ConsoleMaskedLineView(_console, _consoleCursor, options.MaskSymbol);
                case LineViewType.Hidden:
                    return new InMemoryLineView();
                default:
                    throw new NotSupportedException($"{options.ViewType} view is not supported.");
            }
        }
    }
}
