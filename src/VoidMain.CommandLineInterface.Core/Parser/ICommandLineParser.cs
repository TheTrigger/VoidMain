using System.Collections.Generic;
using System.Threading;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.Parser
{
    public interface ICommandLineParser
    {
        CommandLineSyntax Parse(string commandLine, CancellationToken cancellation = default(CancellationToken));
    }
}
