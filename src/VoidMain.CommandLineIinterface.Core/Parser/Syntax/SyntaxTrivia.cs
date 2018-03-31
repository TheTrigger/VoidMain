namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public sealed class SyntaxTrivia : SyntaxNode
    {
        public override TextSpan FullSpan => Span;

        public SyntaxTrivia(SyntaxKind kind, TextSpan span, bool missing = false)
        {
            Kind = kind;
            Span = span;
            IsMissing = missing;
        }
    }
}
