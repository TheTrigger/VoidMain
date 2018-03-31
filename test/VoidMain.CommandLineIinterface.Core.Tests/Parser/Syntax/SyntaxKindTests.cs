using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VoidMain.CommandLineIinterface.Parser.Syntax.Tests
{
    public class SyntaxKindTests
    {
        [Fact]
        public void AllLexerKindsShouldBeMarkedOnlyAsLexerNode()
        {
            var lexerNodeKinds = from kind in GetAllSyntaxKinds()
                                 let name = kind.ToString()
                                 where name.EndsWith("Token") || name.EndsWith("Trivia")
                                 select kind;

            foreach (var kind in lexerNodeKinds)
            {
                Assert.True(kind.HasFlag(SyntaxKind.LexerNode), $"{kind} should cantains LexerNode");
                Assert.True(!kind.HasFlag(SyntaxKind.ParserNode), $"{kind} should not cantains ParserNode");
            }
        }

        [Fact]
        public void AllParserKindsShouldBeMarkedOnlyAsParserNode()
        {
            var parserNodeKinds = from kind in GetAllSyntaxKinds()
                                  let name = kind.ToString()
                                  where name.EndsWith("Syntax")
                                  select kind;

            foreach (var kind in parserNodeKinds)
            {
                Assert.True(kind.HasFlag(SyntaxKind.ParserNode), $"{kind} should cantains ParserNode");
                Assert.True(!kind.HasFlag(SyntaxKind.LexerNode), $"{kind} should not cantains LexerNode");
            }
        }

        [Fact]
        public void SyntaxKindShouldContainsOnlyLexerAndParserKinds()
        {
            var unknownKinds = (from kind in GetAllSyntaxKinds()
                                let name = kind.ToString()
                                where !name.EndsWith("Token")
                                where !name.EndsWith("Trivia")
                                where !name.EndsWith("Syntax")
                                select kind).Except(SpecialKinds).ToArray();

            Assert.True(unknownKinds.Length == 0,
                "Unknown syntax kinds: " + String.Join(", ", unknownKinds));
        }

        [Fact]
        public void QuotedLiteralShouldContainsOnlyLiteral()
        {
            var kinds = GetAllSyntaxKinds()
                .Except(SpecialKinds);

            var matches = kinds.Where(k => SyntaxKind.QuotedLiteralToken.HasFlag(k))
                .Except(new[] { SyntaxKind.QuotedLiteralToken }).ToArray();

            Assert.True(matches.Length == 1, "QuotedLiteralToken should not contains: "
                + String.Join(", ", matches.Except(new[] { SyntaxKind.LiteralToken })));
            Assert.True(SyntaxKind.QuotedLiteralToken.HasFlag(SyntaxKind.LiteralToken));
        }

        [Fact]
        public void IdentifierOrLiteralShouldContainsOnlyIdentifierAndLiteral()
        {
            var kinds = GetAllSyntaxKinds()
                .Except(SpecialKinds);

            var matches = kinds.Where(k => SyntaxKind.IdentifierOrLiteralToken.HasFlag(k))
                .Except(new[] { SyntaxKind.IdentifierOrLiteralToken }).ToArray();

            Assert.True(matches.Length == 2, "IdentifierOrLiteralToken should not contains: "
                + String.Join(", ", matches.Except(new[] { SyntaxKind.IdentifierToken, SyntaxKind.LiteralToken })));
            Assert.True(SyntaxKind.IdentifierOrLiteralToken.HasFlag(SyntaxKind.IdentifierToken));
            Assert.True(SyntaxKind.IdentifierOrLiteralToken.HasFlag(SyntaxKind.LiteralToken));
        }

        [Fact]
        public void AllSingleMeaningKindsShouldNotContainsAnyOtherKind()
        {
            var kinds = GetAllSyntaxKinds()
                .Except(SpecialKinds)
                .Except(DoubleMeaningKinds);

            var matchesByKind = from kind in kinds
                                let matches = kinds.Where(_ => kind != _ && kind.HasFlag(_)).ToArray()
                                select new { kind, matches };

            foreach (var mk in matchesByKind)
            {
                Assert.True(mk.matches.Length == 0,
                    mk.kind + " should not contains: " + String.Join(", ", mk.matches));
            }
        }

        private static readonly IReadOnlyList<SyntaxKind> SpecialKinds
            = new[] { SyntaxKind.Undefined, SyntaxKind.LexerNode, SyntaxKind.ParserNode };

        private static readonly IReadOnlyList<SyntaxKind> DoubleMeaningKinds
            = new[] { SyntaxKind.QuotedLiteralToken, SyntaxKind.IdentifierOrLiteralToken };

        private IEnumerable<SyntaxKind> GetAllSyntaxKinds()
        {
            return Enum.GetValues(typeof(SyntaxKind))
                .Cast<SyntaxKind>();
        }
    }
}
