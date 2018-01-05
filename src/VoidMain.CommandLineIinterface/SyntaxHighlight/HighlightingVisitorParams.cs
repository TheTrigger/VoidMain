using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class HighlightingVisitorParams<TStyle>
    {
        public List<StyledSpan<TStyle>> Spans { get; }
        public SyntaxHighlightingPallete<TStyle> Pallete { get; }

        public HighlightingVisitorParams(List<StyledSpan<TStyle>> spans,
            SyntaxHighlightingPallete<TStyle> pallete)
        {
            Spans = spans;
            Pallete = pallete;
        }
    }
}
