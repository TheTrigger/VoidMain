using System.Runtime.InteropServices;
using VoidMain.Application;
using VoidMain.CommandLineIinterface;
using VoidMain.CommandLineIinterface.IO;
using VoidMain.CommandLineIinterface.IO.InputHandlers;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleInterfaceServiceCollectionExtensions
    {
        public static ConsoleInterfaceBuilder AddConsoleInterfaceCore(
            this IServiceCollection services)
        {
            services.AddSingleton<ICommandLineInterface, ConsoleInterface>();

            services.AddSingleton<ICommandLineOutput, ConsoleLockingOutput>();
            services.AddSingleton<ConsoleOutputLock>();

            var cursorType = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? typeof(CmdCursor)
                : typeof(TerminalCursor);
            services.AddSingleton(typeof(IConsoleCursor), cursorType);
            services.AddSingleton<IConsole>(_ => PlatformConsole.Instance);

            services.AddSingleton<ICommandLineReader, ConsoleCommandLineReader>();
            services.AddSingleton<IConsoleKeyReader, ConsoleKeyReader>();
            services.AddTransient<IConsoleInputHandler, TypeCharacterInputHandler>();
            services.AddTransient<IConsoleInputHandler, DeleteCharacterInputHandler>();
            services.AddTransient<IConsoleInputHandler, MoveCursorInputHandler>();

            services.AddSingleton<ICommandLineViewNavigation, CommandLineViewNavigation>();

            services.AddSingleton<ICommandLineViewProvider, ConsoleCommandLineViewProvider>();

            services.AddSingleton<ICommandLineParser, CommandLineParser>();
            services.AddSingleton<ICommandLineLexer, CommandLineLexer>();
            services.AddSingleton<ISemanticModel, EmptySemanticModel>();
            services.AddSingleton<SyntaxFactory, SyntaxFactory>();

            return new ConsoleInterfaceBuilder(services);
        }

        public static ConsoleInterfaceBuilder AddConsoleInterface(
            this IServiceCollection services)
        {
            var interfaceBuilder = services
                .AddConsoleInterfaceCore()
                .AddPromptMessage()
                .AddUndoRedo()
                .AddSyntaxHighlighting();

            interfaceBuilder
                .AddCommandsHistory()
                .AddFileStorage();

            return interfaceBuilder;
        }
    }
}
