using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Highlighting
{
    public interface ITextHighlighter<TStyle>
    {
        IReadOnlyList<StyledSpan<TStyle>> Highlight(string text);
    }
}
