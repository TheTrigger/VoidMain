using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoidMain.CommandLineIinterface;
using VoidMain.Hosting;

namespace VoidMain.Application.Commands
{
    public class CommandsApplication : ICommandsApplication
    {
        private readonly IServiceProvider _services;
        private readonly ICommandLineOutput _output;

        public CommandsApplication(IServiceProvider services, ICommandLineOutput output)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public Task ExecuteCommand(Dictionary<string, object> context)
        {
            string commandLine = (string)context[ContextKey.CommandLine];
            _output.WriteLine("Execute: " + commandLine);
            return Task.CompletedTask;
        }
    }
}
