using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.Highlighting
{
    public interface ITextHighlighter<TStyle>
    {
        IReadOnlyList<StyledSpan<TStyle>> Highlight(string text);
    }
}
