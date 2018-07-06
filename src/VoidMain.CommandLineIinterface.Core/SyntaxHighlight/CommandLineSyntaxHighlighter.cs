using System;
using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class CommandLineSyntaxHighlighter : ITextHighlighter<TextStyle>
    {
        private readonly ICommandLineParser _parser;
        private readonly ISyntaxHighlighter<TextStyle> _syntaxHighlighter;
        private readonly SyntaxHighlightingOptions _highlightingOptions;

        public CommandLineSyntaxHighlighter(
            ICommandLineParser parser,
            ISyntaxHighlighter<TextStyle> syntaxHighlighter,
            SyntaxHighlightingOptions highlightingOptions = null)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _syntaxHighlighter = syntaxHighlighter ?? throw new ArgumentNullException(nameof(syntaxHighlighter));
            _highlightingOptions = highlightingOptions ?? new SyntaxHighlightingOptions();
            _highlightingOptions.Validate();
        }

        public IReadOnlyList<StyledSpan<TextStyle>> Highlight(string commandLine)
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException(nameof(commandLine));
            }

            if (commandLine.Length == 0)
            {
                return Array.Empty<StyledSpan<TextStyle>>();
            }

            if (String.IsNullOrWhiteSpace(commandLine))
            {
                return new[]
                {
                    new StyledSpan<TextStyle>(
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
