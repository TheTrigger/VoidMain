namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class SyntaxFactory
    {
        public SyntaxTrivia UndefinedTrivia(string input, int position, int length)
        {
            return new SyntaxTrivia(SyntaxKind.Undefined,
                new TextSpan(input, position, length));
        }

        public SyntaxToken EndOfInputToken(string input, int position, bool missing = false)
        {
            return new SyntaxToken(SyntaxKind.EndOfInputToken,
                new TextSpan(input, position, 0), "<eoi>", missing);
        }

        public SyntaxTrivia WhitespaceTrivia(string input, int position, int length, bool missing = false)
        {
            return new SyntaxTrivia(SyntaxKind.WhitespaceTrivia,
                new TextSpan(input, position, length), missing);
        }

        public SyntaxToken DashToken(string input, int position, bool missing = false)
        {
            return new SyntaxToken(SyntaxKind.DashToken,
                new TextSpan(input, position, 1), "-", missing);
        }

        public SyntaxToken DashDashToken(string input, int position, bool missing = false)
        {
            return new SyntaxToken(SyntaxKind.DashDashToken,
                new TextSpan(input, position, 2), "--", missing);
        }

        public SyntaxToken EqualsToken(string input, int position, bool missing = false)
        {
            return new SyntaxToken(SyntaxKind.EqualsToken,
                new TextSpan(input, position, 1), "=", missing);
        }

        public SyntaxToken ColonToken(string input, int position, bool missing = false)
        {
            return new SyntaxToken(SyntaxKind.ColonToken,
                new TextSpan(input, position, 1), ":", missing);
        }

        public SyntaxToken IdentifierOrLiteralToken(string input, int position, int length,
            string stringValue, bool missing = false)
        {
            return new SyntaxToken(SyntaxKind.IdentifierOrLiteralToken,
                new TextSpan(input, position, length), stringValue, missing);
        }

        public SyntaxToken LiteralToken(string input, int position, int length,
            string stringValue, bool quoted = false, bool missing = false)
        {
            return new SyntaxToken(quoted ? SyntaxKind.QuotedLiteralToken : SyntaxKind.LiteralToken,
                new TextSpan(input, position, length), stringValue, missing);
        }
    }
}
