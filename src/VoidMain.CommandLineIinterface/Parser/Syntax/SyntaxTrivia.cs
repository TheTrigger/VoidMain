namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class SyntaxTrivia : SyntaxNode
    {
        public SyntaxTrivia(SyntaxKind kind, TextSpan span)
        {
            Kind = kind;
            Span = span;
        }

        public override bool IsTrivia => true;
        public override TextSpan FullSpan => Span;
    }
}
