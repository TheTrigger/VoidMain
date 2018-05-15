using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class HighlightingVisitorParams<TStyle>
    {
        public List<StyledSpan<TStyle>> Spans { get; }
        public int HighlightedLength { get; set; }
        public SyntaxHighlightingPalette<TStyle> Palette { get; }

        public HighlightingVisitorParams(List<StyledSpan<TStyle>> spans,
            SyntaxHighlightingPalette<TStyle> palette)
        {
            Spans = spans;
            Palette = palette;
        }
    }
}
