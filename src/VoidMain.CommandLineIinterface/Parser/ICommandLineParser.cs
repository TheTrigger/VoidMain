using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Parser
{
    public interface ICommandLineParser
    {
        void ParseToContext(string commandLine, Dictionary<string, object> context);
    }
}
