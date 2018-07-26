using System.Runtime.InteropServices;
using VoidMain.Application;
using VoidMain.CommandLineInterface;
using VoidMain.CommandLineInterface.IO;
using VoidMain.CommandLineInterface.IO.Console;
using VoidMain.CommandLineInterface.IO.InputHandlers;
using VoidMain.CommandLineInterface.IO.Templates;
using VoidMain.CommandLineInterface.IO.Views;
using VoidMain.CommandLineInterface.Parser;
using VoidMain.CommandLineInterface.Parser.Syntax;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleInterfaceServiceCollectionExtensions
    {
        public static ConsoleInterfaceBuilder AddConsoleInterfaceCore(
            this IServiceCollection services)
        {
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            services.AddSingleton<ICommandLineInterface, ConsoleInterface>();

            services.AddSingleton<ICommandLineOutput, ConsoleLockingOutput>();

            if (isWindows)
            {
                if (WindowsVirtualTerminal.TryEnable())
                {
                    services.AddTransient<IColoredTextWriter, ConsoleAnsiColoredTextWriter>();
                }
                else
                {
                    services.AddSingleton<IConsoleColorConverter, NearestConsoleColorConverter>();
                    services.AddTransient<IColoredTextWriter, ConsoleColoredTextWriter>();
                }
            }
            else
            {
                services.AddTransient<IColoredTextWriter, ConsoleAnsiColoredTextWriter>();
            }

            services.AddTransient<IMessageTemplateParser, MessageTemplateParser>();
            services.AddTransient<IMessageTemplateWriter, MessageTemplateWriter>();
            services.AddTransient<IMessageTemplateColoredWriter, MessageTemplateColoredWriter>();
            services.AddTransient<IMessageTemplateValueFormatter, MessageTemplateValueFormatter>();
            services.AddSingleton<ConsoleOutputLock>();

            var cursorType = isWindows
                ? typeof(CmdCursor)
                : typeof(TerminalCursor);
            services.AddSingleton(typeof(IConsoleCursor), cursorType);
            services.AddSingleton<IConsole>(_ => PlatformConsole.Instance);

            services.AddSingleton<ILineReader, ConsoleLineReader>();
            services.AddTransient<ILineViewEditor, LineViewEditor>();
            services.AddTransient<IInputKeyReader, ConsoleKeyReader>();
            services.AddTransient<IConsoleKeyConverter, ConsoleKeyConverter>();
            services.AddTransient<IInputHandler, TypeCharacterInputHandler>();
            services.AddTransient<IInputHandler, DeleteCharacterInputHandler>();
            services.AddTransient<IInputHandler, MoveCursorInputHandler>();

            services.AddSingleton<ILineViewNavigation, ByWordLineViewNavigation>();

            services.AddSingleton<ILineViewProvider, ConsoleLineViewProvider>();

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
