namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public sealed class OptionSyntax : ArgumentSyntax
    {
        public SyntaxToken NameMarker { get; }
        public SyntaxToken Name { get; }
        public SyntaxToken ValueMarker { get; }
        public ValueSyntax Value { get; }

        public OptionSyntax(SyntaxToken nameMarker, SyntaxToken name,
            SyntaxToken valueMarker, ValueSyntax value)
            : base(SyntaxKind.OptionSyntax, new SyntaxNode[] { nameMarker, name, valueMarker, value })
        {
            NameMarker = nameMarker;
            Name = name;
            ValueMarker = valueMarker;
            Value = value;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitOption(this, param);
        }
    }
}
