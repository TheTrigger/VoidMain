namespace VoidMain.CommandLineInterface.Parser.Syntax
{
    public sealed class EndOfOptionsSyntax : ArgumentSyntax
    {
        public SyntaxToken Token { get; }

        public EndOfOptionsSyntax(SyntaxToken token)
            : base(SyntaxKind.EndOfOptionsSyntax, token)
        {
            Token = token;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitEndOfOptions(this, param);
        }
    }
}
