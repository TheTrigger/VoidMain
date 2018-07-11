using System.Collections.Generic;
using System.Threading;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.Parser
{
    public interface ICommandLineLexer
    {
        IEnumerable<SyntaxToken> Lex(string commandLine, CancellationToken cancellation = default(CancellationToken));
    }
}
