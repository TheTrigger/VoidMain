namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public sealed class OperandSyntax : ArgumentSyntax
    {
        public ValueSyntax Value { get; }
        public int Index { get; }

        public OperandSyntax(ValueSyntax value, int index)
            : base(SyntaxKind.OperandSyntax, new[] { value })
        {
            Value = value;
            Index = index;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitOperand(this, param);
        }
    }
}
