﻿using System;

namespace VoidMain.CommandLineInterface.Parser.Syntax
{
    [Flags]
    public enum SyntaxKind
    {
        Undefined = 0b0000_0000_0000_0000_0000_0000_0000,

        LexerNode = 0b0010_0000_0000_0000_0000_0000_0000,
        ParserNode = 0b0001_0000_0000_0000_0000_0000_0000,

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
        CommandLineSyntax = ParserNode | 0b0000_0000_0000_0000_0000_0000_0001,
        CommandNameSyntax = ParserNode | 0b0000_0000_0000_0000_0000_0000_0010,
        ArgumentsSectionSyntax = ParserNode | 0b0000_0000_0000_0000_0000_0000_0100,
        EndOfOptionsSyntax = ParserNode | 0b0000_0000_0000_0000_0000_0000_1000,
        OptionSyntax = ParserNode | 0b0000_0000_0000_0000_0000_0001_0000,
        OperandSyntax = ParserNode | 0b0000_0000_0000_0000_0000_0010_0000,
        ValueSyntax = ParserNode | 0b0000_0000_0000_0000_0000_0100_0000
    }
}
