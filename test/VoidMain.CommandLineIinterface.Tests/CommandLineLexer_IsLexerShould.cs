using System;
using System.Linq;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;
using Xunit;

namespace VoidMain.CommandLineIinterface.Tests
{
    public class CommandLineLexer_IsLexerShould
    {
        [Fact]
        public void RequireSyntaxFactory()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandLineLexer(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("a")]
        [InlineData("a ")]
        public void LexEndOfInputToken(string input)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).Last();

            // Assert
            Assert.Equal(SyntaxKind.EndOfInputToken, token.Kind);
        }

        [Theory]
        [InlineData("-", "-", SyntaxKind.DashToken)]
        [InlineData("--", "--", SyntaxKind.DashDashToken)]
        [InlineData("=", "=", SyntaxKind.EqualsToken)]
        [InlineData(":", ":", SyntaxKind.ColonToken)]
        [InlineData("-1", "-1", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("-1L", "-1L", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("a", "a", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("a_1", "a_1", SyntaxKind.IdentifierOrLiteralToken)]
        public void LexSingleToken(string input, string value, SyntaxKind kind)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).First();

            // Assert
            Assert.Equal(value, token.StringValue);
            Assert.Equal(kind, token.Kind);
        }

        [Theory]
        [InlineData("  -  ", "-", SyntaxKind.DashToken)]
        [InlineData("  --  ", "--", SyntaxKind.DashDashToken)]
        [InlineData("  =  ", "=", SyntaxKind.EqualsToken)]
        [InlineData("  :  ", ":", SyntaxKind.ColonToken)]
        [InlineData("  -1  ", "-1", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("  -1L  ", "-1L", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("  a  ", "a", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("  a_1  ", "a_1", SyntaxKind.IdentifierOrLiteralToken)]
        public void LexSingleTokenWithTrivia(string input, string value, SyntaxKind kind)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).First();

            // Assert
            Assert.Equal(value, token.StringValue);
            Assert.Equal(kind, token.Kind);
            Assert.True(token.HasLeadingTrivia);
            Assert.True(token.HasTrailingTrivia);
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("\" a \"", " a ")]
        [InlineData("\"\"\"a\"\"\"", "\"a\"")]
        [InlineData("\"'a'\"", "'a'")]
        [InlineData("\"\"\"\"", "\"")]
        [InlineData("\"\\\"", "\\")]
        [InlineData("\"a", "a")]
        [InlineData("\" ", " ")]
        [InlineData("\"a=b\"", "a=b")]
        [InlineData("\"a:b\"", "a:b")]
        public void LexQuotedToken(string input, string value)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).First();

            // Assert
            Assert.Equal(value, token.StringValue);
            Assert.Equal(SyntaxKind.IdentifierOrLiteralToken, token.Kind);
        }

        [Theory]
        [InlineData("a=b", "=", SyntaxKind.EqualsToken)]
        [InlineData("a:b", ":", SyntaxKind.ColonToken)]
        [InlineData("a = b", "=", SyntaxKind.EqualsToken)]
        [InlineData("a : b", ":", SyntaxKind.ColonToken)]
        public void LexNameValueSeparatorToken(string input, string value, SyntaxKind kind)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).Skip(1).First();

            // Assert
            Assert.Equal(value, token.StringValue);
            Assert.Equal(kind, token.Kind);
        }

        [Theory]
        [InlineData("-a", "-", SyntaxKind.DashToken, "a", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("-abc", "-", SyntaxKind.DashToken, "abc", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("--a", "--", SyntaxKind.DashDashToken, "a", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("--abc", "--", SyntaxKind.DashDashToken, "abc", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData(":a", ":", SyntaxKind.ColonToken, "a", SyntaxKind.IdentifierOrLiteralToken)]
        [InlineData("=a", "=", SyntaxKind.EqualsToken, "a", SyntaxKind.IdentifierOrLiteralToken)]
        public void LexTokensPair(string input,
            string firstValue, SyntaxKind firstKind,
            string secondValue, SyntaxKind secondKind)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var tokens = lexer.Lex(input).ToArray();
            var first = tokens[0];
            var second = tokens[1];

            // Assert
            Assert.Equal(firstValue, first.StringValue);
            Assert.Equal(firstKind, first.Kind);
            Assert.Equal(secondValue, second.StringValue);
            Assert.Equal(secondKind, second.Kind);
        }
    }
}
