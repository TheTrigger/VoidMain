using Microsoft.Extensions.DependencyInjection;
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
        private readonly StringComparer _comparer;

        public CommandsApplication(IServiceProvider services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _comparer = StringComparer.OrdinalIgnoreCase;
        }

        public async Task ExecuteCommand(Dictionary<string, object> context)
        {
            var commandLine = (string)context[ContextKey.CommandLine];
            var token = (CancellationToken)context[ContextKey.CommandCancelled];
            var output = (ICommandLineOutput)context[ContextKey.Output];

            // TODO: Replace with proper commands.
            if (_comparer.Equals(commandLine, "q") ||
                _comparer.Equals(commandLine, "quit"))
            {
                var app = _services.GetRequiredService<ICommandLineIinterface>();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                app.StopAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            else
            {
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
}
