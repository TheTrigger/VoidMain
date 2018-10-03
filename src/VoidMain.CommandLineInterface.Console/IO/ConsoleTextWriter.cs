using System;
using VoidMain.CommandLineInterface.IO.Console;

namespace VoidMain.CommandLineInterface.IO
{
    public class ConsoleTextWriter : ITextWriter
    {
        protected readonly IConsole _console;

        public ConsoleTextWriter(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void Write(char value) => _console.Write(value);
        public void Write(char value, int count) => _console.Write(value, count);
        public void Write(string value) => _console.Write(value);
        public void Write(object value) => _console.Write(value);
        public void WriteLine() => _console.WriteLine();
    }
}
