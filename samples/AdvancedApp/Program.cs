using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using VoidMain.Application.Builder;
using VoidMain.Application.Commands;
using VoidMain.Application.Commands.Builder;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.SyntaxHighlight;
using VoidMain.CommandLineIinterface.SyntaxHighlight.Console;
using VoidMain.Hosting;

namespace AdvancedApp
{
    class Program : IStartup
    {
        static void Main(string[] args)
        {
            PrintDevelopmentNote();

            var host = new CommandsHostBuilder()
                .UseStartup<Program>()
                .Build();

            host.Run();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var interfaceBuilder = services
                .AddConsoleInterface()
                .AddPromptMessage()
                .AddUndoRedo(options =>
                {
                    options.MaxCount = 10;
                    options.SnapshotsComparer = CommandLineViewSnapshotComparer.IgnoreCursor;
                })
                .AddSyntaxHighlighting(options =>
                {
                    options.Pallete = new ConsoleSyntaxHighlightingPallete()
                    {
                        { SyntaxClass.CommandName, ConsoleColor.Yellow },
                        { SyntaxClass.OptionName, ConsoleColor.Blue, ConsoleColor.Yellow },
                        { SyntaxClass.Operand, new ConsoleTextStyle(ConsoleColor.DarkCyan) }
                        // OptionNameMarker, OptionValueMarker, OptionValue, OperandsSectionMarker
                    };
                    // or
                    options.Pallete = ConsoleSyntaxHighlightingPallete.Default;
                });

            interfaceBuilder
                .AddCommandsHistory(options =>
                {
                    options.MaxCount = 10;
                    options.SavePeriod = TimeSpan.FromSeconds(10);
                    options.CommandsComparer = StringComparer.OrdinalIgnoreCase;
                })
                .AddInMemoryStorage(options =>
                {
                    options.Commands = new[] { "version", "help", "quit" };
                })
                .AddFileStorage(options =>
                {
                    options.FilePath = "history.txt";
                    options.Encoding = Encoding.UTF8;
                });

            services.AddCommands();
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            app.RunCommands(commands =>
            {
                commands.AddStandardCommands();
                commands.AddModule<HelloWorldModule>(module =>
                {
                    module.Name = "";
                    module.Description = "";

                    module.RemoveCommand(m => m.RemovedCommand());
                });
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

    public class HelloWorldModule : CommandsModule
    {
        public void Hello([Operand(DefaultValue = "World")] string name)
        {
            Output.WriteLine($"Hello, {name}!");
        }

        public void RemovedCommand() { }
    }
}
