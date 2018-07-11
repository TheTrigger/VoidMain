using System;
using System.Linq;
using VoidMain.CommandLineInterface.Parser.Syntax;
using Xunit;

namespace VoidMain.CommandLineInterface.Parser.Tests
{
    public class CommandLineLexer_Should
    {
        #region Ctor tests

        [Fact]
        public void RequireSyntaxFactory()
        {
            Assert.Throws<ArgumentNullException>(() => new CommandLineLexer(null));
        }

        #endregion

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
        [InlineData("a", 0, 0)]
        [InlineData(" a ", 1, 1)]
        [InlineData("  a  ", 2, 2)]
        public void LexTokenTrivia(string input, int leading, int trailing)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).First();

            // Assert
            if (leading > 0)
            {
                Assert.True(token.HasLeadingTrivia);
                Assert.Equal(leading, token.LeadingTrivia.Span.Length);
            }
            if (trailing > 0)
            {
                Assert.True(token.HasTrailingTrivia);
                Assert.Equal(trailing, token.TrailingTrivia.Span.Length);
            }
        }

        [Theory]
        [InlineData("-", "-", SyntaxKind.DashToken)]
        [InlineData("--", "--", SyntaxKind.DashDashToken)]
        [InlineData("=", "=", SyntaxKind.EqualsToken)]
        [InlineData(":", ":", SyntaxKind.ColonToken)]
        public void LexSpecialToken(string input, string value, SyntaxKind kind)
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
        [InlineData("-1", "-1")]
        [InlineData("-1L", "-1L")]
        [InlineData("-1.0", "-1.0")]
        [InlineData("-1,0", "-1,0")]
        [InlineData("-1.0e+3", "-1.0e+3")]
        [InlineData("-1.0e-3", "-1.0e-3")]
        [InlineData("-1(", "-1(")]
        [InlineData("-1=", "-1=")]
        [InlineData("-1:", "-1:")]
        [InlineData("-1 ", "-1")]
        [InlineData("-1\"", "-1")]
        [InlineData("-1'", "-1")]
        public void LexNegativeNumberToken(string input, string value)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).First();

            // Assert
            Assert.Equal(value, token.StringValue);
            Assert.Equal(SyntaxKind.LiteralToken, token.Kind);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("a1", "a1")]
        [InlineData("a_b", "a_b")]
        [InlineData("a-b", "a-b")]
        [InlineData("a.b", "a.b")]
        [InlineData("a=", "a")]
        [InlineData("a:", "a")]
        [InlineData("a ", "a")]
        [InlineData("a\"", "a")]
        [InlineData("a'", "a")]
        public void LexPossiblyIdentifierToken(string input, string value)
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
        [InlineData("1", "1")]
        [InlineData("1a", "1a")]
        [InlineData("+a", "+a")]
        [InlineData("a+", "a+")]
        [InlineData("1=", "1=")]
        [InlineData("1:", "1:")]
        [InlineData("1 ", "1")]
        [InlineData("1\"", "1")]
        [InlineData("1'", "1")]
        public void LexLiteralToken(string input, string value)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).First();

            // Assert
            Assert.Equal(value, token.StringValue);
            Assert.Equal(SyntaxKind.LiteralToken, token.Kind);
        }

        [Theory]
        [InlineData("\"a\"", "a")]
        [InlineData("\" a \"", " a ")]
        [InlineData("\"\"\"a\"\"\"", "\"a\"")]
        [InlineData("\"'a'\"", "'a'")]
        [InlineData("\"\"\"\"", "\"")]
        [InlineData("\"a", "a")]
        [InlineData("\" ", " ")]
        [InlineData("\"a=b\"", "a=b")]
        [InlineData("\"a:b\"", "a:b")]
        [InlineData("\"a\"b", "a")]
        [InlineData("\"\\\"", "\\")]
        [InlineData("'a'", "a")]
        [InlineData("' a '", " a ")]
        [InlineData("'''a'''", "'a'")]
        [InlineData("'\"a\"'", "\"a\"")]
        [InlineData("''''", "'")]
        [InlineData("'a", "a")]
        [InlineData("' ", " ")]
        [InlineData("'a=b'", "a=b")]
        [InlineData("'a:b'", "a:b")]
        [InlineData("'a'b", "a")]
        [InlineData("'\\'", "\\")]
        public void LexQuotedLiteralToken(string input, string value)
        {
            // Arrange
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);

            // Act
            var token = lexer.Lex(input).First();

            // Assert
            Assert.Equal(value, token.StringValue);
            Assert.Equal(SyntaxKind.QuotedLiteralToken, token.Kind);
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
