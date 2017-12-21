using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.CommandLineIinterface;
using VoidMain.Hosting;

namespace VoidMain.Application.Commands
{
    public class CommandsApplication : ICommandsApplication
    {
        private readonly IServiceProvider _services;

        public CommandsApplication(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task ExecuteCommand(Dictionary<string, object> context)
        {
            var commandLine = (string)context[ContextKey.CommandLine];
            var token = (CancellationToken)context[ContextKey.CommandCancelled];
            var output = (ICommandLineOutput)context[ContextKey.Output];

            output.WriteLine("Executing: " + commandLine);

            for (int i = 0; i < 3; i++)
            {
                output.Write('.');
                await Task.Delay(1000, token);
            }

            output.WriteLine();
            output.WriteLine("Done");
        }
    }
}
