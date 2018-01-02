namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public sealed class SyntaxToken : SyntaxNode
    {
        public override bool IsToken => true;
        public override TextSpan FullSpan => GetFullSpan();
        public string StringValue { get; }

        public bool HasLeadingTrivia => LeadingTrivia != null;
        public bool HasTrailingTrivia => TrailingTrivia != null;
        public SyntaxTrivia LeadingTrivia { get; }
        public SyntaxTrivia TrailingTrivia { get; }

        public SyntaxToken(SyntaxKind kind, TextSpan span, string value,
            bool missing = false, SyntaxTrivia leadingTrivia = null, SyntaxTrivia trailingTrivia = null)
        {
            Kind = kind;
            Span = span;
            StringValue = value;
            IsMissing = missing;
            LeadingTrivia = leadingTrivia;
            TrailingTrivia = trailingTrivia;
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
