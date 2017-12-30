namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class SyntaxToken : SyntaxNode
    {
        public override bool IsToken => true;
        public override TextSpan FullSpan => GetFullSpan();
        public string StringValue { get; set; }

        public bool HasLeadingTrivia => LeadingTrivia != null;
        public bool HasTrailingTrivia => TrailingTrivia != null;
        public SyntaxTrivia LeadingTrivia { get; set; }
        public SyntaxTrivia TrailingTrivia { get; set; }

        public SyntaxToken(SyntaxKind kind, TextSpan span, string value, bool missing = false)
        {
            Kind = kind;
            Span = span;
            StringValue = value;
            IsMissing = missing;
        }

        private TextSpan GetFullSpan()
        {
            var start = HasLeadingTrivia
                ? LeadingTrivia.Span
                : Span;
            var end = HasTrailingTrivia
                ? TrailingTrivia.Span
                : Span;

            return TextSpan.RangeInclusive(start, end);
        }
    }
}
