using System;
using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class SyntaxHighlighter<TStyle> : ISyntaxHighlighter<TStyle>
    {
        private readonly HighlightingVisitor<TStyle> _visitor;

        public SyntaxHighlighter()
        {
            _visitor = new HighlightingVisitor<TStyle>();
        }

        public IReadOnlyList<StyledSpan<TStyle>> GetHighlightedSpans(CommandLineSyntax syntax, SyntaxHighlightingPalette<TStyle> palette)
        {
            if (syntax == null) throw new ArgumentNullException(nameof(syntax));
            if (palette == null) throw new ArgumentNullException(nameof(palette));

            var styledSpans = new List<StyledSpan<TStyle>>();
            var visitorParams = new HighlightingVisitorParams<TStyle>(styledSpans, palette);

            syntax.Accept(_visitor, visitorParams);

            int tailLength = syntax.FullSpan.Length - visitorParams.HighlightedLength;
            if (tailLength > 0)
            {
                var tail = new TextSpan(syntax.FullSpan.Source, visitorParams.HighlightedLength, tailLength);
                styledSpans.Add(new StyledSpan<TStyle>(tail, palette.DefaultStyle));
            }

            return styledSpans;
        }
    }
}
