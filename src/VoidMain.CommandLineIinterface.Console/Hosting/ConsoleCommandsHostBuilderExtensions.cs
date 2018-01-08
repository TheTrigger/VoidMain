using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using VoidMain.CommandLineIinterface;
using VoidMain.CommandLineIinterface.Console;
using VoidMain.CommandLineIinterface.History;
using VoidMain.CommandLineIinterface.History.Console;
using VoidMain.CommandLineIinterface.IO;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.IO.Console.InputHandlers;
using VoidMain.CommandLineIinterface.IO.Console.Internal;
using VoidMain.CommandLineIinterface.IO.Navigation;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.IO.Views.Console;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;
using VoidMain.CommandLineIinterface.SyntaxHighlight;
using VoidMain.CommandLineIinterface.SyntaxHighlight.Console;
using VoidMain.CommandLineIinterface.UndoRedo;

namespace VoidMain.Hosting
{
    public static class ConsoleCommandsHostBuilderExtensions
    {
        public static ICommandsHostBuilder UseConsole(this ICommandsHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.AddConsole();
                services.AddSingleton<ICommandLineIinterface, ConsoleInterface>();
                services.AddCommandLineReader();
                services.AddUndoRedo();
                services.AddCommandsHistory();
                services.AddParser();
                services.AddSyntaxHighlight();
            });
        }

        private static void AddConsole(this IServiceCollection services)
        {
            services.AddSingleton<IConsole>(_ => PlatformConsole.Instance);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.AddSingleton<IConsoleCursor, CmdCursor>();
            }
            else
            {
                services.AddSingleton<IConsoleCursor, TerminalCursor>();
            }
        }

        private static void AddCommandLineReader(this IServiceCollection services)
        {
            services.AddSingleton<ICommandLineReader, ConsoleCommandLineReader>();
            services.AddSingleton<IPrompt, CmdPrompt>();
            services.AddSingleton<ICommandLineViewProvider, ConsoleCommandLineViewProvider>();
            services.AddSingleton<ICommandLineFastNavigation, CommandLineFastNavigation>();
            services.AddTransient<IConsoleInputHandler, TypeCharacterInputHandler>();
            services.AddTransient<IConsoleInputHandler, DeleteCharacterInputHandler>();
            services.AddTransient<IConsoleInputHandler, MoveCursorInputHandler>();
        }

        private static void AddUndoRedo(this IServiceCollection services)
        {
            services.AddTransient<IConsoleInputHandler, UndoRedoInputHandler>();
            services.AddSingleton<IUndoRedoManager, UndoRedoManager>();
        }

        private static void AddCommandsHistory(this IServiceCollection services)
        {
            services.AddTransient<IConsoleInputHandler, CommandsHistoryInputHandler>();
            services.AddSingleton<ICommandsHistoryManager, CommandsHistoryManager>();
            services.AddSingleton<ICommandsHistoryStorage, CommandsHistoryFileStorage>();
        }

        private static void AddParser(this IServiceCollection services)
        {
            services.AddSingleton<ICommandLineParser, CommandLineParser>();
            services.AddSingleton<ICommandLineLexer, CommandLineLexer>();
            services.AddSingleton<ISemanticModel, EmptySemanticModel>();
            services.AddSingleton<SyntaxFactory, SyntaxFactory>();
        }

        private static void AddSyntaxHighlight(this IServiceCollection services)
        {
            services.AddSingleton<ISyntaxHighlighter<ConsoleTextStyle>, SyntaxHighlighter<ConsoleTextStyle>>();
            services.AddSingleton<SyntaxHighlightingPallete<ConsoleTextStyle>, DefaultConsoleSyntaxHighlightingPallete>();
        }
    }
}
