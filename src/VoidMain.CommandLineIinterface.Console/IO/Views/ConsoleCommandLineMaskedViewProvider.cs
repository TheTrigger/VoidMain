using System;
using VoidMain.CommandLineIinterface.IO.Console.Internal;

namespace VoidMain.CommandLineIinterface.IO.Views.Console
{
    public class ConsoleCommandLineMaskedViewProvider : ICommandLineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;

        public CommandLineViewType ViewType { get; } = CommandLineViewType.Masked;

        public ConsoleCommandLineMaskedViewProvider(
            IConsole console, IConsoleCursor cursor)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
        }

        public ICommandLineView GetView(CommandLineViewOptions options)
        {
            return new ConsoleCommandLineMaskedView(_console, _cursor, options.MaskSymbol);
        }
    }
}
