using System.Collections.Generic;
using System.Threading;
using VoidMain.CommandLineInterface.Parser.Syntax;

namespace VoidMain.CommandLineInterface.Parser
{
    public interface ICommandLineParser
    {
        CommandLineSyntax Parse(string commandLine, CancellationToken cancellation = default(CancellationToken));
    }
}
