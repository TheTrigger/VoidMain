using System.Collections.Generic;
using System.Threading;
using VoidMain.CommandLineInterface.Parser.Syntax;

namespace VoidMain.CommandLineInterface.Parser
{
    public interface ICommandLineLexer
    {
        IEnumerable<SyntaxToken> Lex(string commandLine, CancellationToken cancellation = default);
    }
}
