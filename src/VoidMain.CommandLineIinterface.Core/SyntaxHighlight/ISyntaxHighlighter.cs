using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public interface ISyntaxHighlighter<TStyle>
    {
        IReadOnlyList<StyledSpan<TStyle>> GetHighlightedSpans(CommandLineSyntax syntax, SyntaxHighlightingPallete<TStyle> pallete);
    }
}
