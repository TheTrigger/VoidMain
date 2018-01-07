using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
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

        public StyledSpan(string value, TStyle style)
        {
            Span = new TextSpan(value);
            Style = style;
        }

        public override string ToString()
        {
            return $"\"{Span}\" {Style}";
        }
    }
}
