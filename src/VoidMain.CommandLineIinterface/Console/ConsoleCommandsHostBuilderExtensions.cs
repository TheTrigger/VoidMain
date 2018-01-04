using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;
using VoidMain.CommandLineIinterface;
using VoidMain.CommandLineIinterface.Console;
using VoidMain.CommandLineIinterface.Console.ConsoleCursors;
using VoidMain.CommandLineIinterface.Console.InputHandlers;
using VoidMain.CommandLineIinterface.History;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.Hosting
{
    public static class ConsoleCommandsHostBuilderExtensions
    {
        public static ICommandsHostBuilder UseConsole(this ICommandsHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IConsole>(_ => PlatformConsole.Instance);
                services.AddConsoleCursor();

                services.AddSingleton<ICommandLineIinterface, ConsoleInterface>();

                services.AddSingleton<ICommandLineReader, ConsoleCommandLineReader>();
                services.AddSingleton<IPrompt, CmdPrompt>();
                services.AddSingleton<ICommandLineViewProvider, ConsoleCommandLineViewProvider>();
                services.AddTransient<IConsoleInputHandler, TypeCharacterInputHandler>();
                services.AddTransient<IConsoleInputHandler, DeleteCharacterInputHandler>();
                services.AddTransient<IConsoleInputHandler, MoveCursorInputHandler>();
                services.AddTransient<IConsoleInputHandler, CommandsHistoryInputHandler>();

                services.AddSingleton<ICommandLineFastNavigation, CommandLineFastNavigation>();

                services.AddSingleton<ICommandsHistoryManager, CommandsHistoryManager>();
                services.AddSingleton<ICommandsHistoryStorage, FileCommandsHistoryStorage>();

                services.AddSingleton<ICommandLineParser, CommandLineParser>();
                services.AddSingleton<ICommandLineLexer, CommandLineLexer>();
                services.AddSingleton<ISemanticModel, EmptySemanticModel>();
                services.AddSingleton<SyntaxFactory, SyntaxFactory>();

                services.AddSingleton<ISyntaxHighlighter<ConsoleColor?>, SyntaxHighlighter<ConsoleColor?>>();
                services.AddSingleton<SyntaxPallete<ConsoleColor?>, DefaultConsoleSyntaxPallete>();
            });
        }

        private static void AddConsoleCursor(this IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.AddSingleton<IConsoleCursor, CmdCursor>();
            }
            else
            {
                services.AddSingleton<IConsoleCursor, TerminalCursor>();
            }
        }
    }
}
