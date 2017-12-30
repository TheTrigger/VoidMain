namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class OperandSyntax : SyntaxTreeNode
    {
        public SyntaxToken Value { get; }

        public OperandSyntax(SyntaxToken value)
            : base(SyntaxKind.OperandSyntax, new[] { value })
        {
            Value = value;
        }

        protected override bool AcceptSelf<TParam>(ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitOperand(this, param);
        }
    }
}
