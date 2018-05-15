using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public interface ITextHighlighter<TStyle>
    {
        IReadOnlyList<StyledSpan<TStyle>> Highlight(string text);
    }
}
