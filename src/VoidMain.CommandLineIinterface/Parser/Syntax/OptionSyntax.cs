namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class OptionSyntax : SyntaxTreeNode
    {
        public SyntaxToken NameMarker { get; }
        public SyntaxToken Name { get; }
        public SyntaxToken ValueMarker { get; }
        public SyntaxToken Value { get; }

        public OptionSyntax(SyntaxToken nameMarker, SyntaxToken name, SyntaxToken valueMarker, SyntaxToken value)
            : base(SyntaxKind.OptionSyntax, new[] { nameMarker, name, valueMarker, value })
        {
            NameMarker = nameMarker;
            Name = name;
            ValueMarker = valueMarker;
            Value = value;
        }

        protected override bool AcceptSelf<TParam>(ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitOption(this, param);
        }
    }
}
