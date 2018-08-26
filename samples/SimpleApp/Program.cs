using Microsoft.Extensions.DependencyInjection;
using System;
using VoidMain.Application.Builder;
using VoidMain.Application.Commands;
using VoidMain.Application.Commands.Builder;
using VoidMain.CommandLineInterface;
using VoidMain.Hosting;

namespace SimpleApp
{
    [Module(ExcludeFromCommandName = true)]
    public class ExampleModule : CommandsModule
    {
        public void Hello([Operand] string name = "World")
        {
            Output.WriteLine($"Hello, {name}!");
        }

        [Command(Name = "command name")]
        public void Command(string[] operands, string option, bool flag = false)
        {
            Output.WriteLine("Operands: " + String.Join(", ", operands));
            Output.WriteLine("Option: " + option);
            Output.WriteLine("Flag: " + flag);
        }
    }

    class Program : IStartup
    {
        static void Main(string[] args)
        {
            CliApp.Run<Program>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsoleInterface();
            services.AddCommands();
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            PrintWelcomeMessage(app);

            app.UseHelpCommandsRewriter();
            app.RunCommands(commands =>
            {
                commands.AddStandardCommands();
                commands.AddHelpCommands();
                commands.AddModule<ExampleModule>();
            });
        }

        private void PrintWelcomeMessage(IApplicationBuilder app)
        {
            var output = app.Services.GetRequiredService<ICommandLineOutput>();

            string welcomeMessage =
@"=============================================================
 This framework is still in the early development.
 See {0} to learn what features are available.
 Type {1} or press {2} twice to close the application.
=============================================================";

            output.WriteLine(new ColoredFormat(welcomeMessage)
            {
                { "README.md", Color.DarkCyan },
                { "quit", Color.Yellow },
                { " Ctrl+C ", Color.Black, Color.Gray }
            });
            output.WriteLine();
        }
    }
}
