namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public enum SyntaxKind
    {
        Undefined,

        // Lexer nodes
        WhitespaceTrivia,
        DashToken,
        DashDashToken,
        EqualsToken,
        ColonToken,
        IdentifierOrLiteralToken,
        EndOfInputToken,

        // Parser nodes
        CommandLineSyntax,
        CommandNameSyntax,
        OptionsSectionSyntax,
        OperandsSectionSyntax,
        OptionSyntax,
        OperandSyntax
    }
}
