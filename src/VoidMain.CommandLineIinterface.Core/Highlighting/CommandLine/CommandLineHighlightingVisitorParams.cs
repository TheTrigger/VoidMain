using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Highlighting.CommandLine
{
    public class CommandLineHighlightingVisitorParams<TStyle>
    {
        public List<StyledSpan<TStyle>> Spans { get; }
        public HighlightingPalette<CommandLineStyleName, TStyle> Palette { get; }

        public CommandLineHighlightingVisitorParams(List<StyledSpan<TStyle>> spans,
            HighlightingPalette<CommandLineStyleName, TStyle> palette)
        {
            Spans = spans;
            Palette = palette;
        }
    }
}
