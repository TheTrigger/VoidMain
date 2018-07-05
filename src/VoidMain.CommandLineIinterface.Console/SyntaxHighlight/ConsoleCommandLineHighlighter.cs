using System;
using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class ConsoleCommandLineHighlighter : ITextHighlighter<ConsoleTextStyle>
    {
        private readonly ICommandLineParser _parser;
        private readonly ISyntaxHighlighter<ConsoleTextStyle> _syntaxHighlighter;
        private readonly ConsoleSyntaxHighlightingOptions _highlightingOptions;

        public ConsoleCommandLineHighlighter(
            ICommandLineParser parser,
            ISyntaxHighlighter<ConsoleTextStyle> syntaxHighlighter,
            ConsoleSyntaxHighlightingOptions highlightingOptions = null)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _syntaxHighlighter = syntaxHighlighter ?? throw new ArgumentNullException(nameof(syntaxHighlighter));
            _highlightingOptions = highlightingOptions ?? new ConsoleSyntaxHighlightingOptions();
            _highlightingOptions.Validate();
        }

        public IReadOnlyList<StyledSpan<ConsoleTextStyle>> Highlight(string commandLine)
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException(nameof(commandLine));
            }

            if (commandLine.Length == 0)
            {
                return Array.Empty<StyledSpan<ConsoleTextStyle>>();
            }

            if (String.IsNullOrWhiteSpace(commandLine))
            {
                return new[]
                {
                    new StyledSpan<ConsoleTextStyle>(
                        new TextSpan(commandLine),
                        _highlightingOptions.Palette.DefaultStyle)
                };
            }

            var syntax = _parser.Parse(commandLine);
            var highlights = _syntaxHighlighter.GetHighlightedSpans(syntax, _highlightingOptions.Palette);
            return highlights;
        }
    }
}
