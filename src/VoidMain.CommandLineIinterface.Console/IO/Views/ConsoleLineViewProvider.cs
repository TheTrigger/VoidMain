using System;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleLineViewProvider : ILineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;

        public ConsoleLineViewProvider(IConsole console, IConsoleCursor cursor)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
        }

        public ILineView GetView(LineViewOptions options)
        {
            switch (options.ViewType)
            {
                case LineViewType.Normal:
                    return new ConsoleLineView(_console, _cursor);
                case LineViewType.Masked:
                    return new ConsoleMaskedLineView(_console, _cursor, options.MaskSymbol);
                case LineViewType.Hidden:
                    return new InMemoryLineView();
                default:
                    throw new NotSupportedException($"{options.ViewType} view is not supported.");
            }
        }
    }
}
