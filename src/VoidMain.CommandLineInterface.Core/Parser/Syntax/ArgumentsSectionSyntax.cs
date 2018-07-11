using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.Parser.Syntax
{
    public sealed class ArgumentsSectionSyntax : SyntaxTreeNode
    {
        public IReadOnlyList<ArgumentSyntax> Arguments { get; }

        public ArgumentsSectionSyntax(IReadOnlyList<ArgumentSyntax> arguments)
            : base(SyntaxKind.ArgumentsSectionSyntax, arguments)
        {
            Arguments = arguments;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitArgumentsSection(this, param);
        }
    }
}
