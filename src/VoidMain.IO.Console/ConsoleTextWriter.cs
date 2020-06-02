using System;
using VoidMain.Text;

namespace VoidMain.IO.Console
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
        public void Write(string? value) => _console.Write(value);
        public void Write(ReadOnlySpan<char> value) => _console.Write(value);
        public void WriteNewLine() => _console.WriteLine();
    }
}
