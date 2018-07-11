using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public abstract class ArgumentSyntax : SyntaxTreeNode
    {
        protected ArgumentSyntax(SyntaxKind kind, SyntaxNode child)
            : base(kind, child)
        {
        }

        protected ArgumentSyntax(SyntaxKind kind, IEnumerable<SyntaxNode> children)
            : base(kind, children)
        {
        }
    }
}
