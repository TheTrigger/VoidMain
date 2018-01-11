using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface;
using VoidMain.Hosting;

namespace VoidMain.Application.Commands
{
    public class CommandsApplication : ICommandsApplication
    {
        private readonly IServiceProvider _services;
        private readonly ApplicationModel _appModel;
        private readonly StringComparer _comparerComparer;

        public CommandsApplication(IServiceProvider services, ApplicationModel appModel)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _appModel = appModel ?? throw new ArgumentNullException(nameof(appModel));
            _comparerComparer = StringComparer.OrdinalIgnoreCase;
        }

        public async Task ExecuteCommand(Dictionary<string, object> context)
        {
            var commandLine = (string)context[ContextKey.CommandLine];
            var token = (CancellationToken)context[ContextKey.CommandCanceled];
            var output = (ICommandLineOutput)context[ContextKey.Output];

            bool handled = HandleQuitCommand(context);
            if (handled) return;

            PrintCommandDetails(context, output);

            output.Write("Executing command");

            for (int i = 0; i < 3; i++)
            {
                output.Write('.');
                await Task.Delay(1000, token);
            }

            output.WriteLine("\nDone\n");
        }

        // TODO: Replace with proper commands.
        private bool HandleQuitCommand(Dictionary<string, object> context)
        {
            var commandName = (string[])context[ContextKey.CommandName];
            if (commandName == null || commandName.Length == 0) return false;

            bool isQuitCommand = _comparerComparer.Equals(commandName[0], "q")
                || _comparerComparer.Equals(commandName[0], "quit");
            if (!isQuitCommand) return false;

#pragma warning disable CS4014 // Do not await it because app will wait until this method is over.
            var app = _services.GetRequiredService<ICommandLineIinterface>();
            app.StopAsync().ContinueWith(t => { /* TODO: Log error. */ }, TaskContinuationOptions.OnlyOnFaulted);
#pragma warning restore CS4014

            return true;
        }

        private void PrintCommandDetails(
            Dictionary<string, object> context, ICommandLineOutput output)
        {
            output.WriteLine("Command info");
            output.WriteLine("============");

            output.WriteLine("Command name:");
            var commandName = (string[])context[ContextKey.CommandName];
            if (commandName == null || commandName.Length == 0)
            {
                output.WriteLine("\t---");
            }
            else
            {
                foreach (var name in commandName)
                {
                    output.Write('\t');
                    output.WriteLine(name);
                }
            }

            output.WriteLine("Options:");
            var options = (Dictionary<string, string[]>)context[ContextKey.CommandOptions];
            if (options == null || options.Count == 0)
            {
                output.WriteLine("\t---");
            }
            else
            {
                foreach (var option in options)
                {
                    string values = option.Value.Length == 1
                           ? option.Value[0] ?? "<NULL>"
                           : "[" + String.Join(", ", option.Value.Select(_ => _ ?? "<NULL>")) + "]";

                    output.Write('\t');
                    output.Write(option.Key);
                    output.Write(": ");
                    output.WriteLine(values);
                }
            }

            output.WriteLine("Operands:");
            var operands = (string[])context[ContextKey.CommandOperands];
            if (operands == null || operands.Length == 0)
            {
                output.WriteLine("\t---");
            }
            else
            {
                foreach (var operand in operands)
                {
                    output.Write('\t');
                    output.WriteLine(operand ?? "<NULL>");
                }
            }
        }
    }
}
