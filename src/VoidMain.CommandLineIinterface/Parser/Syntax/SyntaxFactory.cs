namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class SyntaxFactory
    {
        public SyntaxToken EndOfInputToken(string input, int position, bool missing = false)
        {
            return new SyntaxToken(SyntaxKind.EndOfInputToken,
                new TextSpan(input, position, 0), "<eoi>", missing);
        }

        public SyntaxToken DashToken(string input, int position)
        {
            return new SyntaxToken(SyntaxKind.DashToken,
                new TextSpan(input, position, 1), "-");
        }

        public SyntaxToken DashDashToken(string input, int position)
        {
            return new SyntaxToken(SyntaxKind.DashDashToken,
                new TextSpan(input, position, 2), "--");
        }

        public SyntaxToken EqualsToken(string input, int position)
        {
            return new SyntaxToken(SyntaxKind.EqualsToken,
                new TextSpan(input, position, 1), "=");
        }

        public SyntaxToken ColonToken(string input, int position)
        {
            return new SyntaxToken(SyntaxKind.ColonToken,
                new TextSpan(input, position, 1), ":");
        }

        public SyntaxToken IdentifierOrLiteralToken(string input, int position, int length, string stringValue)
        {
            return new SyntaxToken(SyntaxKind.IdentifierOrLiteralToken,
                new TextSpan(input, position, length), stringValue);
        }

        public SyntaxTrivia WhitespaceTrivia(string input, int position, int length)
        {
            return new SyntaxTrivia(SyntaxKind.WhitespaceTrivia,
                new TextSpan(input, position, length));
        }
    }
}
