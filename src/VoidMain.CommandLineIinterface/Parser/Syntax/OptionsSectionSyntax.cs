using System.Collections.Generic;
using System.Linq;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public class OptionsSectionSyntax : SyntaxTreeNode
    {
        public IReadOnlyList<OptionSyntax> Options { get; }

        public OptionsSectionSyntax(IReadOnlyList<OptionSyntax> options)
            : base(SyntaxKind.OptionsSectionSyntax, options)
        {
            Options = options.ToArray();
        }

        protected override bool AcceptSelf<TParam>(ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitOptionsSection(this, param);
        }
    }
}
