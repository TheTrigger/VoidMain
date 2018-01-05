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

        public IReadOnlyList<StyledSpan<TStyle>> GetHighlightedSpans(CommandLineSyntax syntax, SyntaxHighlightingPallete<TStyle> pallete)
        {
            if (syntax == null) throw new ArgumentNullException(nameof(syntax));
            if (pallete == null) throw new ArgumentNullException(nameof(pallete));

            var styledSpans = new List<StyledSpan<TStyle>>();
            var visitorParams = new HighlightingVisitorParams<TStyle>(styledSpans, pallete);

            syntax.Accept(_visitor, visitorParams);
            ValidateSpans(styledSpans);

            return styledSpans;
        }

        private void ValidateSpans(List<StyledSpan<TStyle>> spans)
        {
            int pos = 0;

            foreach (var highlight in spans)
            {
                if (highlight.Span.Start < pos)
                {
                    throw new Exception("Overlapped spans");
                }
                pos = highlight.Span.End;
            }
        }
    }
}
