using System;

namespace VoidMain.CommandLineIinterface.Console
{
    public class CommandLineOutput : ICommandLineOutput
    {
        private readonly IConsole _console;

        public CommandLineOutput(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void Write(char value) => _console.Write(value);
        public void Write(string value) => _console.Write(value);
        public void Write(object value) => _console.Write(value);
        public void Write(string format, params object[] args) => _console.Write(format, args);

        public void WriteLine() => _console.WriteLine();
        public void WriteLine(string value) => _console.WriteLine(value);
        public void WriteLine(object value) => _console.WriteLine(value);
        public void WriteLine(string format, params object[] args) => _console.WriteLine(format, args);

        public void Clear() => _console.Clear();
    }
}
