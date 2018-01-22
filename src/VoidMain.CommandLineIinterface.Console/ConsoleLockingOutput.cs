using System;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.IO.Console.Internal;

namespace VoidMain.CommandLineIinterface.Console
{

    public class ConsoleLockingOutput : ICommandLineOutput
    {
        private readonly IConsole _console;
        private readonly ConsoleOutputLock _lock;

        public ConsoleLockingOutput(IConsole console, ConsoleOutputLock @lock)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
        }

        public void Write(char value)
        {
            _lock.ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(string value)
        {
            _lock.ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(object value)
        {
            _lock.ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(string format, params object[] args)
        {
            _lock.ThrowIfLocked();
            _console.Write(format, args);
        }

        public void WriteLine()
        {
            _lock.ThrowIfLocked();
            _console.WriteLine();
        }

        public void WriteLine(string value)
        {
            _lock.ThrowIfLocked();
            _console.WriteLine(value);
        }

        public void WriteLine(object value)
        {
            _lock.ThrowIfLocked();
            _console.WriteLine(value);
        }

        public void WriteLine(string format, params object[] args)
        {
            _lock.ThrowIfLocked();
            _console.WriteLine(format, args);
        }

        public void Clear()
        {
            _lock.ThrowIfLocked();
            _console.Clear();
        }
    }
}
