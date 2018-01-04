using System;
using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class SyntaxHighlighter<TColor> : ISyntaxHighlighter<TColor>
    {
        private readonly HighlightingVisitor<TColor> _visitor;

        public SyntaxHighlighter()
        {
            _visitor = new HighlightingVisitor<TColor>();
        }

        public IReadOnlyList<ColoredSpan<TColor>> GetColoredSpans(CommandLineSyntax syntax, SyntaxPallete<TColor> pallete)
        {
            if (syntax == null) throw new ArgumentNullException(nameof(syntax));
            if (pallete == null) throw new ArgumentNullException(nameof(pallete));

            var coloredSpans = new List<ColoredSpan<TColor>>();
            var visitorParams = new HighlightingVisitorParams<TColor>(coloredSpans, pallete);
            syntax.Accept(_visitor, visitorParams);
            ValidateSpans(coloredSpans);
            return coloredSpans;
        }

        private void ValidateSpans(List<ColoredSpan<TColor>> spans)
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
