namespace VoidMain.CommandLineInterface.Parser.Syntax
{
    public sealed class OperandSyntax : ArgumentSyntax
    {
        public int Index { get; }
        public ValueSyntax Value { get; }

        public OperandSyntax(int index, ValueSyntax value)
            : base(SyntaxKind.OperandSyntax, new[] { value })
        {
            Index = index;
            Value = value;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitOperand(this, param);
        }
    }
}
