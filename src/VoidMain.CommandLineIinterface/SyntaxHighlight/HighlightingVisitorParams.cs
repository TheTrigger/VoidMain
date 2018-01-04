using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class HighlightingVisitorParams<TColor>
    {
        public List<ColoredSpan<TColor>> Spans { get; }
        public SyntaxPallete<TColor> Pallete { get; }

        public HighlightingVisitorParams(List<ColoredSpan<TColor>> spans, SyntaxPallete<TColor> pallete)
        {
            Spans = spans;
            Pallete = pallete;
        }
    }
}
