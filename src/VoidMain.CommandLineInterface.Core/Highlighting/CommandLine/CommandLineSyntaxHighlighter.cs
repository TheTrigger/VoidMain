using System;
using System.Collections.Generic;
using VoidMain.CommandLineInterface.Parser;
using VoidMain.CommandLineInterface.Parser.Syntax;

namespace VoidMain.CommandLineInterface.Highlighting.CommandLine
{
    public class CommandLineSyntaxHighlighter : ITextHighlighter<TextStyle>
    {
        private readonly ICommandLineParser _parser;
        private readonly CommandLineHighlightingOptions _highlightingOptions;
        private readonly CommandLineHighlightingVisitor<TextStyle> _highlightingVisitor;

        public CommandLineSyntaxHighlighter(
            ICommandLineParser parser,
            CommandLineHighlightingOptions highlightingOptions = null)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _highlightingOptions = highlightingOptions ?? new CommandLineHighlightingOptions();
            _highlightingOptions.Validate();

            _highlightingVisitor = new CommandLineHighlightingVisitor<TextStyle>();
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
                return GetDefaultForAll(commandLine);
            }

            var syntax = _parser.Parse(commandLine);

            var highlights = new List<StyledSpan<TextStyle>>();
            var visitorParams = new CommandLineHighlightingVisitorParams<TextStyle>(
                highlights, _highlightingOptions.Palette);

            syntax.Accept(_highlightingVisitor, visitorParams);

            return highlights;
        }

        private IReadOnlyList<StyledSpan<TextStyle>> GetDefaultForAll(string text)
        {
            var span = new TextSpan(text);
            var style = _highlightingOptions.Palette.DefaultStyle;
            return new[] { new StyledSpan<TextStyle>(span, style) };
        }
    }
}
