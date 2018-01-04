using System;
using VoidMain.CommandLineIinterface.Console.Views;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleCommandLineViewProvider : ICommandLineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly ICommandLineParser _parser;
        private readonly ISyntaxHighlighter<ConsoleColor?> _highlighter;
        private readonly SyntaxPallete<ConsoleColor?> _pallete;

        public ConsoleCommandLineViewProvider(
            IConsole console, IConsoleCursor cursor, ICommandLineParser parser,
            ISyntaxHighlighter<ConsoleColor?> highlighter, SyntaxPallete<ConsoleColor?> pallete)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _highlighter = highlighter ?? throw new ArgumentNullException(nameof(highlighter));
            _pallete = pallete ?? throw new ArgumentNullException(nameof(pallete));
        }

        public ICommandLineView GetView(CommandLineViewOptions options)
        {
            // TODO: Reuse line views.
            switch (options.ViewType)
            {
                case CommandLineViewType.Normal:
                    // return new ConsoleCommandLineView(_console, _cursor);
                    return new ConsoleCommandLineHighlightedView(_console, _cursor, _parser, _highlighter, _pallete);
                case CommandLineViewType.Masked:
                    return new ConsoleCommandLineMaskedView(_console, _cursor, options.MaskSymbol);
                case CommandLineViewType.Hidden:
                    return new ConsoleCommandLineHiddenView();
                default:
                    throw new ArgumentOutOfRangeException("Unknown view type " + options.ViewType);
            }
        }
    }
}
