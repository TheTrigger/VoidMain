using System.Collections.Generic;
using System.Linq;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class CommandNameSyntax : SyntaxTreeNode
    {
        public IReadOnlyList<SyntaxNode> NameParts { get; }

        public CommandNameSyntax(IReadOnlyList<SyntaxNode> nameParts)
            : base(SyntaxKind.CommandNameSyntax, nameParts)
        {
            NameParts = nameParts.ToArray();
        }

        protected override bool AcceptSelf<TParam>(ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitCommandName(this, param);
        }
    }
}
