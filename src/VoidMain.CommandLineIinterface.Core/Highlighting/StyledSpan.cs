using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.Highlighting
{
    public class StyledSpan<TStyle>
    {
        public TextSpan Span { get; }
        public TStyle Style { get; }

        public StyledSpan(TextSpan span, TStyle style)
        {
            Span = span;
            Style = style;
        }

        public override string ToString()
        {
            return $"\"{Span}\" {Style}";
        }
    }
}
