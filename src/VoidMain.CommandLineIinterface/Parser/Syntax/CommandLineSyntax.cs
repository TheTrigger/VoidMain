namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class CommandLineSyntax : SyntaxTreeNode
    {
        public CommandNameSyntax CommandName { get; }
        public OptionsSectionSyntax Options { get; }
        public OperandsSectionSyntax Operands { get; }
        public SyntaxToken EndOfInput { get; }

        public CommandLineSyntax(CommandNameSyntax commandName,
            OptionsSectionSyntax options, OperandsSectionSyntax operands,
            SyntaxToken endOfInput)
            : base(SyntaxKind.CommandLineSyntax, new SyntaxNode[] { commandName, options, operands, endOfInput })
        {
            CommandName = commandName;
            Options = options;
            Operands = operands;
            EndOfInput = endOfInput;
        }

        protected override bool AcceptSelf<TParam>(ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitCommandLine(this, param);
        }
    }
}
