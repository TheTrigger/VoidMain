using System;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleLockingOutput : ICommandLineOutput
    {
        private readonly IConsole _console;
        private bool _isLocked;

        public ConsoleLockingOutput(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _isLocked = false;
        }

        public void LockForRead() => _isLocked = true;
        public void Unlock() => _isLocked = false;

        private void ThrowIfLocked()
        {
            if (_isLocked)
            {
                throw new InvalidOperationException("Output is locked during a read operation.");
            }
        }

        public void Write(char value)
        {
            ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(string value)
        {
            ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(object value)
        {
            ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(string format, params object[] args)
        {
            ThrowIfLocked();
            _console.Write(format, args);
        }

        public void WriteLine()
        {
            ThrowIfLocked();
            _console.WriteLine();
        }

        public void WriteLine(string value)
        {
            ThrowIfLocked();
            _console.WriteLine(value);
        }

        public void WriteLine(object value)
        {
            ThrowIfLocked();
            _console.WriteLine(value);
        }

        public void WriteLine(string format, params object[] args)
        {
            ThrowIfLocked();
            _console.WriteLine(format, args);
        }

        public void Clear()
        {
            ThrowIfLocked();
            _console.Clear();
        }
    }
}
