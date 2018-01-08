using System.Runtime.InteropServices;
using VoidMain.CommandLineIinterface;
using VoidMain.CommandLineIinterface.Console;
using VoidMain.CommandLineIinterface.IO;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.IO.Console.InputHandlers;
using VoidMain.CommandLineIinterface.IO.Console.Internal;
using VoidMain.CommandLineIinterface.IO.Navigation;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.IO.Views.Console;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleInterfaceServiceCollectionExtensions
    {
        public static ConsoleInterfaceBuilder AddConsoleInterface(
            this IServiceCollection services)
        {
            services.AddSingleton<ICommandLineIinterface, ConsoleInterface>();

            var cursorType = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? typeof(CmdCursor)
                : typeof(TerminalCursor);
            services.AddSingleton(typeof(IConsoleCursor), cursorType);
            services.AddSingleton<IConsole>(_ => PlatformConsole.Instance);

            services.AddSingleton<ICommandLineReader, ConsoleCommandLineReader>();
            services.AddTransient<IConsoleInputHandler, TypeCharacterInputHandler>();
            services.AddTransient<IConsoleInputHandler, DeleteCharacterInputHandler>();
            services.AddTransient<IConsoleInputHandler, MoveCursorInputHandler>();

            services.AddSingleton<ICommandLineFastNavigation, CommandLineFastNavigation>();

            services.AddSingleton<ICommandLineViewProvider, ConsoleCommandLineViewProvider>();

            services.AddSingleton<ICommandLineParser, CommandLineParser>();
            services.AddSingleton<ICommandLineLexer, CommandLineLexer>();
            services.AddSingleton<ISemanticModel, EmptySemanticModel>();
            services.AddSingleton<SyntaxFactory, SyntaxFactory>();

            return new ConsoleInterfaceBuilder(services);
        }

        public static ConsoleInterfaceBuilder AddAdvancedConsoleInterface(
            this IServiceCollection services)
        {
            var interfaceBuilder = services
                .AddConsoleInterface()
                .AddCmdPrompt()
                .AddUndoRedo();

            interfaceBuilder
                .AddCommandsHistory()
                .AddFileStorage();

            return interfaceBuilder;
        }
    }
}
