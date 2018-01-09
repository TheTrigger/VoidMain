﻿using System;
using VoidMain.CommandLineIinterface.IO.Console.Internal;

namespace VoidMain.CommandLineIinterface.IO.Views.Console
{
    public class ConsoleCommandLineViewProvider : ICommandLineViewProvider
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;

        public CommandLineViewType ViewType { get; } = CommandLineViewType.Normal;

        public ConsoleCommandLineViewProvider(
            IConsole console, IConsoleCursor cursor)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
        }

        public ICommandLineView GetView(CommandLineViewOptions options)
        {
            return new ConsoleCommandLineView(_console, _cursor);
        }
    }
}
