using System;
using VoidMain.CommandLineInterface.IO.Console;

namespace VoidMain.CommandLineInterface.IO.Views
{
    public class ConsoleLineViewProvider : ILineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _consoleCursor;

        public ConsoleLineViewProvider(IConsole console, IConsoleCursor consoleCursor)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _consoleCursor = consoleCursor ?? throw new ArgumentNullException(nameof(consoleCursor));
        }

        public ILineView GetView(LineViewOptions options)
        {
            switch (options.ViewType)
            {
                case LineViewType.Normal:
                    return new ConsoleLineView(_console, _consoleCursor);
                case LineViewType.Masked:
                    return new ConsoleMaskedLineView(_console, _consoleCursor, options.MaskSymbol);
                case LineViewType.Hidden:
                    return new InMemoryLineView();
                default:
                    throw new NotSupportedException($"{options.ViewType} view is not supported.");
            }
        }
    }
}
