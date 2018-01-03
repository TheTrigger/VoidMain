using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public sealed class ValueSyntax : SyntaxTreeNode
    {
        public IReadOnlyList<SyntaxToken> Tokens { get; }

        public ValueSyntax(SyntaxToken token)
            : base(SyntaxKind.ValueSyntax, token)
        {
            Tokens = new[] { token };
        }

        public ValueSyntax(IReadOnlyList<SyntaxToken> tokens)
            : base(SyntaxKind.ValueSyntax, tokens)
        {
            Tokens = tokens;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitValue(this, param);
        }
    }
}
