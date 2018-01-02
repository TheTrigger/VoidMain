namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class SyntaxTokenBuilder
    {
        public SyntaxKind Kind { get; set; }
        public TextSpan Span { get; set; }
        public bool IsMissing { get; set; }
        public string StringValue { get; set; }
        public SyntaxTrivia LeadingTrivia { get; set; }
        public SyntaxTrivia TrailingTrivia { get; set; }

        public void InitializeWith(SyntaxToken token)
        {
            Kind = token.Kind;
            Span = token.Span;
            IsMissing = token.IsMissing;
            StringValue = token.StringValue;
            LeadingTrivia = token.LeadingTrivia;
            TrailingTrivia = token.TrailingTrivia;
        }

        public void InitializeWith(SyntaxKind kind,
            TextSpan span, string value, bool missing = false)
        {
            Kind = kind;
            Span = span;
            StringValue = value;
            IsMissing = missing;
        }

        public SyntaxToken Build()
        {
            return new SyntaxToken(Kind, Span, StringValue,
                IsMissing, LeadingTrivia, TrailingTrivia);
        }
    }
}
