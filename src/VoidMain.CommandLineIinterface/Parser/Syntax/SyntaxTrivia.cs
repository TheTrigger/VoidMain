namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class SyntaxTrivia : SyntaxNode
    {
        public SyntaxTrivia(SyntaxKind kind, TextSpan span, bool missing = false)
        {
            Kind = kind;
            Span = span;
            IsMissing = IsMissing;
        }

        public override bool IsTrivia => true;
        public override TextSpan FullSpan => Span;
    }
}
