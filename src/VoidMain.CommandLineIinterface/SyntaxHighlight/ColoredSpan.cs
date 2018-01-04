using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class ColoredSpan<TColor>
    {
        public TextSpan Span { get; }
        public TColor Background { get; }
        public TColor Foreground { get; }

        public ColoredSpan(TextSpan span, TColor background, TColor foreground)
        {
            Span = span;
            Background = background;
            Foreground = foreground;
        }

        public override string ToString()
        {
            return $"{Span} [{Background}, {Foreground}]";
        }
    }
}
