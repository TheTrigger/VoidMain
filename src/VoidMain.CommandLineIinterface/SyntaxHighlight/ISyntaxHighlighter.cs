using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public interface ISyntaxHighlighter<TColor>
    {
        IReadOnlyList<ColoredSpan<TColor>> GetColoredSpans(CommandLineSyntax syntax, SyntaxPallete<TColor> pallete);
    }
}
