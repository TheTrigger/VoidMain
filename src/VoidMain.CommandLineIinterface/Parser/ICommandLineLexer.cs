using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.Parser
{
    public interface ICommandLineLexer
    {
        IEnumerable<SyntaxToken> Lex(string commandLine);
    }
}
