using Microsoft.Extensions.DependencyInjection;
using System;
using VoidMain.Application.Builder;
using VoidMain.Application.Commands;
using VoidMain.Application.Commands.Builder;
using VoidMain.Hosting;

namespace SimpleApp
{
    [Module(Name = "")]
    public class ExampleModule : CommandsModule
    {
        public void Hello([Operand] string name = "World")
        {
            Output.WriteLine($"Hello, {name}!");
        }

        [Command(Name = "command name")]
        public void Command(string option, bool flag, string[] operands)
        {
            Output.WriteLine("Command was executed.");
        }
    }

    class Program : IStartup
    {
        static void Main(string[] args)
        {
            PrintDevelopmentNote();
            CliApp.Run<Program>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsoleInterface();
            services.AddCommands();
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseHelpCommandsRewriter();
            app.RunCommands(commands =>
            {
                commands.AddStandardCommands();
                commands.AddHelpCommands();
                commands.AddModule<ExampleModule>();
            });
        }

        private static void PrintDevelopmentNote()
        {
            Console.WriteLine("=======================================================");
            Console.WriteLine("This framework is still in early development.");
            Console.WriteLine("See README.md to learn what features are available.");
            Console.WriteLine("Type 'quit' or press Ctrl+C twice to close application.");
            Console.WriteLine("=======================================================");
            Console.WriteLine();
        }
    }
}
