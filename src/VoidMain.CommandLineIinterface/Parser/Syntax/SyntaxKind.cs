using System;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    [Flags]
    public enum SyntaxKind
    {
        Undefined = 0b0000_0000_0000_0000_0000_0000_0000,

        LexerNode = 0b1100_0000_0000_0000_0000_0000_0000,
        ParserNode = 0b1010_0000_0000_0000_0000_0000_0000,

        // Lexer nodes
        EndOfInputToken = 0b1100_0000_0000_0000_0000_0000_0001,
        WhitespaceTrivia = 0b1100_0000_0000_0000_0000_0000_0010,
        LiteralToken = 0b1100_0000_0000_0000_0000_0000_0100,
        QuotedLiteralToken = LiteralToken | 0b1100_0000_0000_0000_0000_0000_1000,
        IdentifierToken = 0b1100_0000_0000_0000_0000_0001_0000,
        IdentifierOrLiteralToken = IdentifierToken | LiteralToken,
        DashToken = 0b1100_0000_0000_0000_0000_0010_0000,
        DashDashToken = 0b1100_0000_0000_0000_0000_0100_0000,
        EqualsToken = 0b1100_0000_0000_0000_0000_1000_0000,
        ColonToken = 0b1100_0000_0000_0000_0001_0000_0000,

        // Parser nodes
        CommandLineSyntax = 0b1010_1000_0000_0000_0000_0000_0000,
        CommandNameSyntax = 0b1010_0100_0000_0000_0000_0000_0000,
        OptionsSectionSyntax = 0b1010_0010_0000_0000_0000_0000_0000,
        OperandsSectionSyntax = 0b1010_0001_0000_0000_0000_0000_0000,
        OptionSyntax = 0b1010_0000_1000_0000_0000_0000_0000,
        OperandSyntax = 0b1010_0000_0100_0000_0000_0000_0000
    }
}
