﻿using System;
using VoidMain.CommandLineIinterface.IO.Console.Internal;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.SyntaxHighlight;
using VoidMain.CommandLineIinterface.SyntaxHighlight.Console;

namespace VoidMain.CommandLineIinterface.IO.Views.Console
{
    public class ConsoleCommandLineHighlightedViewProvider : ICommandLineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly ICommandLineParser _parser;
        private readonly ISyntaxHighlighter<ConsoleTextStyle> _highlighter;
        private readonly SyntaxHighlightingPallete<ConsoleTextStyle> _pallete;

        public ConsoleCommandLineHighlightedViewProvider(
            IConsole console, IConsoleCursor cursor,
            ICommandLineParser parser, ISyntaxHighlighter<ConsoleTextStyle> highlighter,
            SyntaxHighlightingOptions<ConsoleTextStyle> options = null)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _highlighter = highlighter ?? throw new ArgumentNullException(nameof(highlighter));
            _pallete = options?.Pallete ?? ConsoleSyntaxHighlightingPallete.Default;
        }

        public ICommandLineView GetView(CommandLineViewOptions options)
        {
            switch (options.ViewType)
            {
                case CommandLineViewType.Normal:
                    return new ConsoleCommandLineHighlightedView(_console, _cursor, _parser, _highlighter, _pallete);
                case CommandLineViewType.Masked:
                    return new ConsoleCommandLineMaskedView(_console, _cursor, options.MaskSymbol);
                case CommandLineViewType.Hidden:
                    return new CommandLineHiddenView();
                default:
                    throw new NotSupportedException($"{options.ViewType} view is not supported.");
            }
        }
    }
}
