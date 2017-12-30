using System.Collections.Generic;
using System.Linq;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class OperandsSectionSyntax : SyntaxTreeNode
    {
        public SyntaxToken SectionMarker { get; }
        public IReadOnlyList<OperandSyntax> Operands { get; }

        public OperandsSectionSyntax(SyntaxToken sectionMarker, IReadOnlyList<OperandSyntax> operands)
            : base(SyntaxKind.OperandsSectionSyntax, Concat(sectionMarker, operands))
        {
            SectionMarker = sectionMarker;
            Operands = operands.ToArray();
        }

        private static SyntaxNode[] Concat(SyntaxNode first, IReadOnlyList<SyntaxNode> rest)
        {
            var nodes = new SyntaxNode[rest.Count + 1];

            nodes[0] = first;
            for (int i = 0; i < rest.Count; i++)
            {
                nodes[i + 1] = rest[i];
            }

            return nodes;
        }

        protected override bool AcceptSelf<TParam>(ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitOperandsSection(this, param);
        }
    }
}
