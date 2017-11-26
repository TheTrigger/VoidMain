using System.Collections.Generic;
using VoidMain.Hosting;

namespace VoidMain.CommandLineIinterface.Parser
{

    public class CommandLineParser : ICommandLineParser
    {
        public void ParseToContext(string commandLine, Dictionary<string, object> context)
        {
            context[ContextKey.CommandLine] = commandLine;
            context[ContextKey.CommandName] = commandLine.Split(' ');
            context[ContextKey.CommandOptions] = new Dictionary<string, string[]>();
            context[ContextKey.CommandOperands] = new Dictionary<string, string[]>();
        }
    }
}
