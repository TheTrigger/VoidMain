using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using VoidMain.CommandLineIinterface;
using VoidMain.CommandLineIinterface.Console;
using VoidMain.CommandLineIinterface.Console.ConsoleCursors;
using VoidMain.CommandLineIinterface.Console.InputHandlers;
using VoidMain.CommandLineIinterface.History;
using VoidMain.CommandLineIinterface.Parser;

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
                services.AddSingleton<ICommandLineOutput, ConsoleLockingOutput>();

                services.AddTransient<ICommandLineReader, ConsoleCommandLineReader>();
                services.AddTransient<IPrompt, CmdPrompt>();
                services.AddTransient<ICommandLineViewManager, ConsoleCommandLineView>();
                services.AddTransient<IConsoleInputHandler, TypeCharacterInputHandler>();
                services.AddTransient<IConsoleInputHandler, DeleteCharacterInputHandler>();
                services.AddTransient<IConsoleInputHandler, MoveCursorInputHandler>();
                services.AddTransient<IConsoleInputHandler, CommandsHistoryInputHandler>();

                services.AddSingleton<ICommandLineFastNavigation, CommandLineFastNavigation>();

                services.AddSingleton<ICommandsHistoryManager, CommandsHistoryManager>();
                services.AddSingleton<ICommandsHistoryStorage, InMemoryCommandsHistoryStorage>();

                services.AddTransient<ICommandLineParser, CommandLineParser>();
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
