using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.Parser.Syntax
{
    public sealed class CommandLineSyntax : SyntaxTreeNode
    {
        public CommandNameSyntax CommandName { get; }
        public ArgumentsSectionSyntax Arguments { get; }
        public SyntaxToken EndOfInput { get; }
        public IReadOnlyList<SyntaxError> Errors { get; }
        public bool HasErrors => Errors?.Count > 0;

        public CommandLineSyntax(CommandNameSyntax commandName, ArgumentsSectionSyntax arguments,
            SyntaxToken endOfInput, IReadOnlyList<SyntaxError> errors)
            : base(SyntaxKind.CommandLineSyntax, new SyntaxNode[] { commandName, arguments, endOfInput })
        {
            CommandName = commandName;
            Arguments = arguments;
            EndOfInput = endOfInput;
            Errors = errors;
        }

        protected override bool AcceptSelf<TParam>(
            ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            return visitor.VisitCommandLine(this, param);
        }
    }
}
