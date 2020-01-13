using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using VoidMain.Application.Builder;
using VoidMain.Application.Commands;
using VoidMain.Application.Commands.Builder;
using VoidMain.CommandLineInterface;
using VoidMain.CommandLineInterface.Highlighting;
using VoidMain.CommandLineInterface.Highlighting.CommandLine;
using VoidMain.CommandLineInterface.IO.Views;
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
                    options.SnapshotsComparer = LineViewSnapshotComparer.IgnoreCursor;
                })
                .AddSyntaxHighlighting(options =>
                {
                    options.Palette = new CommandLineHighlightingPalette()
                    {
                        { CommandLineStyleName.CommandName, Colors.Yellow },
                        { CommandLineStyleName.OptionName, Colors.Blue, Colors.Yellow },
                        { CommandLineStyleName.Operand, new TextStyle(Colors.DarkCyan) }
                        // OptionNameMarker, OptionValueMarker, OptionValue, EndOfOptions
                    };
                    // or
                    options.Palette = CommandLineHighlightingPalette.Default;
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
            PrintWelcomeMessage(app);

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
                { "README.md", Colors.DarkCyan },
                { "quit", Colors.Yellow },
                { " Ctrl+C ", Colors.Black, Colors.Gray }
            });
            output.WriteLine();
        }
    }
}
