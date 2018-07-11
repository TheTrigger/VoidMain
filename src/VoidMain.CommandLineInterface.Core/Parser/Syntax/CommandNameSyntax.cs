using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public sealed class CommandNameSyntax : SyntaxTreeNode
    {
        public IReadOnlyList<SyntaxToken> NameParts { get; }

        public CommandNameSyntax(IReadOnlyList<SyntaxToken> nameParts)
            : base(SyntaxKind.CommandNameSyntax, nameParts)
        {
            NameParts = nameParts;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitCommandName(this, param);
        }
    }
}
