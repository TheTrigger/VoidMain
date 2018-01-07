using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoidMain.CommandLineIinterface;
using VoidMain.CommandLineIinterface.Console;
using VoidMain.CommandLineIinterface.History;
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
                services.AddTransient<IConsoleInputHandler, UndoRedoInputHandler>();

                services.AddSingleton<ICommandLineFastNavigation, CommandLineFastNavigation>();

                services.AddSingleton<ICommandsHistoryManager, CommandsHistoryManager>();
                services.AddSingleton<ICommandsHistoryStorage, CommandsHistoryFileStorage>();
                services.AddTransient<ICommandsHistoryEqualityComparer, CommandsHistoryOrdinalIgnoreCaseEqualityComparer>();

                services.AddSingleton<IUndoRedoManager<CommandLineViewSnapshot>, UndoRedoManager<CommandLineViewSnapshot>>();
                services.AddSingleton<IEqualityComparer<CommandLineViewSnapshot>, CommandLineViewSnapshotWithoutCursorEqualityComparer>();

                services.AddSingleton<ICommandLineParser, CommandLineParser>();
                services.AddSingleton<ICommandLineLexer, CommandLineLexer>();
                services.AddSingleton<ISemanticModel, EmptySemanticModel>();
                services.AddSingleton<SyntaxFactory, SyntaxFactory>();

                services.AddSingleton<ISyntaxHighlighter<ConsoleTextStyle>, SyntaxHighlighter<ConsoleTextStyle>>();
                services.AddSingleton<SyntaxHighlightingPallete<ConsoleTextStyle>, DefaultConsoleSyntaxHighlightingPallete>();
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
