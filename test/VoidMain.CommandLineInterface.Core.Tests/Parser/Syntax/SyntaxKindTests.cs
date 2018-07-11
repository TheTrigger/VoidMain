using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VoidMain.CommandLineInterface.Parser.Syntax.Tests
{
    public class SyntaxKindTests
    {
        [Fact]
        public void AllLexerKindsShouldBeMarkedOnlyAsLexerNode()
        {
            bool IsTokenOrTrivia(SyntaxKind kind) => IsToken(kind) || IsTrivia(kind);

            var lexerNodeKinds = GetAllKinds().Where(IsTokenOrTrivia);

            foreach (var kind in lexerNodeKinds)
            {
                Assert.True(kind.HasFlag(SyntaxKind.LexerNode), $"{kind} should contains LexerNode");
                Assert.True(!kind.HasFlag(SyntaxKind.ParserNode), $"{kind} should not contains ParserNode");
            }
        }

        [Fact]
        public void AllParserKindsShouldBeMarkedOnlyAsParserNode()
        {
            var parserNodeKinds = GetAllKinds().Where(IsSyntax);

            foreach (var kind in parserNodeKinds)
            {
                Assert.True(kind.HasFlag(SyntaxKind.ParserNode), $"{kind} should contains ParserNode");
                Assert.True(!kind.HasFlag(SyntaxKind.LexerNode), $"{kind} should not contains LexerNode");
            }
        }

        [Fact]
        public void ShouldContainsOnlyLexerAndParserKinds()
        {
            bool NotTokenOrTriviaOrSyntax(SyntaxKind kind)
                => !IsToken(kind) && !IsTrivia(kind) && !IsSyntax(kind);

            var unknownKinds = GetAllKinds()
                .Where(NotTokenOrTriviaOrSyntax)
                .Except(SpecialKinds).ToArray();

            Assert.True(unknownKinds.Length == 0,
                "Unknown syntax kinds: " + String.Join(", ", unknownKinds));
        }

        [Fact]
        public void QuotedLiteralShouldContainsOnlyLiteral()
        {
            var kinds = GetAllKinds()
                .Except(SpecialKinds);

            var quotedLiteral = SyntaxKind.QuotedLiteralToken;

            var matches = kinds.Except(new[] { quotedLiteral })
                .Where(k => quotedLiteral.HasFlag(k))
                .ToArray();


            var expected = new[] { SyntaxKind.LiteralToken };
            Assert.True(Equals(expected, matches),
                quotedLiteral + " should not contains: " + Format(matches.Except(expected)));
        }

        [Fact]
        public void IdentifierOrLiteralShouldContainsOnlyIdentifierAndLiteral()
        {
            var kinds = GetAllKinds()
                .Except(SpecialKinds);

            var identifierOrLiteral = SyntaxKind.IdentifierOrLiteralToken;

            var matches = kinds.Except(new[] { identifierOrLiteral })
                .Where(k => identifierOrLiteral.HasFlag(k))
                .ToArray();

            var expected = new[] { SyntaxKind.IdentifierToken, SyntaxKind.LiteralToken };
            Assert.True(Equals(expected, matches),
                identifierOrLiteral + " should not contains: " + Format(matches.Except(expected)));
        }

        [Fact]
        public void AllSingleMeaningKindsShouldNotContainsAnyOtherKind()
        {
            var kinds = GetAllKinds()
                .Except(SpecialKinds)
                .Except(DoubleMeaningKinds)
                .ToArray();

            var matchesByKind = from kind in kinds
                                let matches = kinds.Where(k => kind != k && kind.HasFlag(k)).ToArray()
                                select new { kind, matches };

            foreach (var mk in matchesByKind)
            {
                Assert.True(mk.matches.Length == 0,
                    mk.kind + " should not contains: " + Format(mk.matches));
            }
        }

        private static readonly IReadOnlyList<SyntaxKind> SpecialKinds
            = new[] { SyntaxKind.Undefined, SyntaxKind.LexerNode, SyntaxKind.ParserNode };

        private static readonly IReadOnlyList<SyntaxKind> DoubleMeaningKinds
            = new[] { SyntaxKind.QuotedLiteralToken, SyntaxKind.IdentifierOrLiteralToken };

        private IEnumerable<SyntaxKind> GetAllKinds()
            => Enum.GetValues(typeof(SyntaxKind)).Cast<SyntaxKind>();

        private const string TokenSuffix = "Token";
        private const string TriviaSuffix = "Trivia";
        private const string SyntaxSuffix = "Syntax";

        private static bool IsToken(SyntaxKind kind) => kind.ToString().EndsWith(TokenSuffix);
        private static bool IsTrivia(SyntaxKind kind) => kind.ToString().EndsWith(TriviaSuffix);
        private static bool IsSyntax(SyntaxKind kind) => kind.ToString().EndsWith(SyntaxSuffix);

        private static string Format(IEnumerable<SyntaxKind> kinds) => String.Join(", ", kinds);

        private static bool Equals(IEnumerable<SyntaxKind> a, IEnumerable<SyntaxKind> b)
            => Enumerable.SequenceEqual(a.OrderBy(_ => _), b.OrderBy(_ => _));
    }
}
