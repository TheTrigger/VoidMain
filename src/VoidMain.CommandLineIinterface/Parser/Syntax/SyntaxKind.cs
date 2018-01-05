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
        EndOfInputToken = LexerNode | 0b0000_0000_0000_0000_0000_0000_0001,
        WhitespaceTrivia = LexerNode | 0b0000_0000_0000_0000_0000_0000_0010,
        LiteralToken = LexerNode | 0b0000_0000_0000_0000_0000_0000_0100,
        QuotedLiteralToken = LiteralToken | 0b0000_0000_0000_0000_0000_0000_1000,
        IdentifierToken = LexerNode | 0b0000_0000_0000_0000_0000_0001_0000,
        IdentifierOrLiteralToken = IdentifierToken | LiteralToken,
        DashToken = LexerNode | 0b0000_0000_0000_0000_0000_0010_0000,
        DashDashToken = LexerNode | 0b0000_0000_0000_0000_0000_0100_0000,
        EqualsToken = LexerNode | 0b0000_0000_0000_0000_0000_1000_0000,
        ColonToken = LexerNode | 0b0000_0000_0000_0000_0001_0000_0000,

        // Parser nodes
        CommandLineSyntax = ParserNode | 0b0000_1000_0000_0000_0000_0000_0000,
        CommandNameSyntax = ParserNode | 0b0000_0100_0000_0000_0000_0000_0000,
        ArgumentsSectionSyntax = ParserNode | 0b0000_0010_0000_0000_0000_0000_0000,
        OperandsSectionMarkerSyntax = ParserNode | 0b0000_0001_0000_0000_0000_0000_0000,
        OptionSyntax = ParserNode | 0b0000_0000_1000_0000_0000_0000_0000,
        OperandSyntax = ParserNode | 0b0000_0000_0100_0000_0000_0000_0000,
        ValueSyntax = ParserNode | 0b0000_0000_0010_0000_0000_0000_0000
    }
}
