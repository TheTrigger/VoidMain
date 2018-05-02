using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using VoidMain.Application.Builder;
using VoidMain.Application.Commands;
using VoidMain.Application.Commands.Builder;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.SyntaxHighlight;
using VoidMain.Hosting;

namespace AdvancedApp
{
    public class ExampleModule : CommandsModule
    {
        [Command(Description = "Greet the specified person")]
        public void Hello(
            [Operand(Description = "Name of the person")] string name = "World")
        {
            Output.WriteLine($"Hello, {name}!");
        }

        public void RemovedCommand() { }
    }

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
            services.AddConsoleInterfaceCore()
                .AddPromptMessage("CMD> ")
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
                })
                .AddCommandsHistory(options =>
                {
                    options.MaxCount = 10;
                    options.SavePeriod = TimeSpan.FromSeconds(10);
                    options.CommandsComparer = StringComparer.Ordinal;
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
            app.UseHelpCommandsRewriter();
            app.RunCommands(commands =>
            {
                commands.AddStandardCommands();
                commands.AddHelpCommands();
                commands.AddModule<ExampleModule>(module =>
                {
                    module.Name = "Example";
                    module.Description = "Example module";
                    module.ExcludeFromCommandName = true;

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
}
