namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public sealed class OperandsSectionMarkerSyntax : ArgumentSyntax
    {
        public SyntaxToken SectionMarker { get; }

        public OperandsSectionMarkerSyntax(SyntaxToken sectionMarker)
            : base(SyntaxKind.OperandsSectionMarkerSyntax, sectionMarker)
        {
            SectionMarker = sectionMarker;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitOperandsSectionMarker(this, param);
        }
    }
}
