using System;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleHighlightedLineViewProvider : ILineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly ICommandLineParser _parser;
        private readonly ISyntaxHighlighter<ConsoleTextStyle> _highlighter;
        private readonly ConsoleSyntaxHighlightingOptions _highlightingOptions;

        public ConsoleHighlightedLineViewProvider(
            IConsole console, IConsoleCursor cursor,
            ICommandLineParser parser, ISyntaxHighlighter<ConsoleTextStyle> highlighter,
            ConsoleSyntaxHighlightingOptions highlightingOptions = null)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _highlighter = highlighter ?? throw new ArgumentNullException(nameof(highlighter));
            _highlightingOptions = highlightingOptions ?? new ConsoleSyntaxHighlightingOptions();
            _highlightingOptions.Validate();
        }

        public ILineView GetView(LineViewOptions options)
        {
            switch (options.ViewType)
            {
                case LineViewType.Normal:
                    return new ConsoleHighlightedLineView(_console, _cursor, _parser, _highlighter, _highlightingOptions.Pallete);
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
