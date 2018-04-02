using System;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleCommandLineViewProvider : ICommandLineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;

        public ConsoleCommandLineViewProvider(IConsole console, IConsoleCursor cursor)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
        }

        public ICommandLineView GetView(CommandLineViewOptions options)
        {
            switch (options.ViewType)
            {
                case CommandLineViewType.Normal:
                    return new ConsoleCommandLineView(_console, _cursor);
                case CommandLineViewType.Masked:
                    return new ConsoleCommandLineMaskedView(_console, _cursor, options.MaskSymbol);
                case CommandLineViewType.Hidden:
                    return new CommandLineInMemoryView();
                default:
                    throw new NotSupportedException($"{options.ViewType} view is not supported.");
            }
        }
    }
}
