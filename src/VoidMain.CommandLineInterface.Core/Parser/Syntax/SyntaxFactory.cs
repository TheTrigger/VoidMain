﻿namespace VoidMain.CommandLineInterface.Parser.Syntax
{
    public class SyntaxFactory
    {
        public SyntaxTrivia UndefinedTrivia(string input,
            int position, int length)
        {
            return new SyntaxTrivia(SyntaxKind.Undefined,
                new TextSpan(input, position, length));
        }

        public SyntaxTrivia WhitespaceTrivia(string input,
            int position, int length, bool missing = false)
        {
            return new SyntaxTrivia(SyntaxKind.WhitespaceTrivia,
                new TextSpan(input, position, length), missing);
        }

        public SyntaxTrivia WhitespaceTriviaAfter(
            SyntaxToken token, int length, bool missing = false)
        {
            var span = token.Span;
            return WhitespaceTrivia(span.Source, span.End, length, missing);
        }

        public void EndOfInputToken(SyntaxTokenBuilder builder,
            string input, int position, bool missing = false)
        {
            builder.InitializeWith(SyntaxKind.EndOfInputToken,
                new TextSpan(input, position, 0), "<eoi>", missing);
        }

        public void DashToken(SyntaxTokenBuilder builder,
            string input, int position, bool missing = false)
        {
            builder.InitializeWith(SyntaxKind.DashToken,
                new TextSpan(input, position, 1), "-", missing);
        }

        public void DashDashToken(SyntaxTokenBuilder builder,
            string input, int position, bool missing = false)
        {
            builder.InitializeWith(SyntaxKind.DashDashToken,
                new TextSpan(input, position, 2), "--", missing);
        }

        public void EqualsToken(SyntaxTokenBuilder builder,
            string input, int position, bool missing = false)
        {
            builder.InitializeWith(SyntaxKind.EqualsToken,
                new TextSpan(input, position, 1), "=", missing);
        }

        public void ColonToken(SyntaxTokenBuilder builder,
            string input, int position, bool missing = false)
        {
            builder.InitializeWith(SyntaxKind.ColonToken,
                new TextSpan(input, position, 1), ":", missing);
        }

        public void IdentifierOrLiteralToken(SyntaxTokenBuilder builder,
            string input, int position, int length, string stringValue,
            bool missing = false)
        {
            builder.InitializeWith(SyntaxKind.IdentifierOrLiteralToken,
                new TextSpan(input, position, length), stringValue, missing);
        }

        public void IdentifierToken(SyntaxTokenBuilder builder,
            string input, int position, int length, string stringValue,
            bool missing = false)
        {
            builder.InitializeWith(SyntaxKind.IdentifierToken,
                new TextSpan(input, position, length), stringValue, missing);
        }

        public void LiteralToken(SyntaxTokenBuilder builder,
            string input, int position, int length, string stringValue,
            bool quoted = false, bool missing = false)
        {
            builder.InitializeWith(quoted ? SyntaxKind.QuotedLiteralToken : SyntaxKind.LiteralToken,
                new TextSpan(input, position, length), stringValue, missing);
        }

        public SyntaxError UnrecognizedInputError(TextSpan span)
        {
            return new SyntaxError(span, "WM1001", "Unrecognized input");
        }

        public SyntaxError MissingEndOfInputError(TextSpan span)
        {
            return new SyntaxError(span, "WM2001", "End of input is missing");
        }

        public SyntaxError MissingWhitespaceError(TextSpan span)
        {
            return new SyntaxError(span, "WM2002", "Whitespace is missing");
        }

        public SyntaxError MissingOptionValueError(TextSpan span)
        {
            return new SyntaxError(span, "WM2003", "Option value is missing");
        }

        public SyntaxError MissingClosingQuoteError(TextSpan span)
        {
            return new SyntaxError(span, "WM2004", "Closing quotation mark is missing");
        }

        public SyntaxError InvalidOptionNameError(TextSpan span)
        {
            return new SyntaxError(span, "WM3001", "Invalid option name");
        }
    }
}
