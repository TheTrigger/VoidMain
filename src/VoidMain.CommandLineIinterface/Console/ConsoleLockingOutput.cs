using System;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleLockingOutput : ICommandLineOutput
    {
        private const string LockErrorMessage = "Output is locked during a read operation.";

        private readonly IConsole _console;
        private bool _isLocked;

        public ConsoleLockingOutput(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _isLocked = false;
        }

        public void LockForRead() => _isLocked = true;
        public void Unlock() => _isLocked = false;

        public void Write(char value)
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.Write(value);
        }

        public void Write(string value)
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.Write(value);
        }

        public void Write(object value)
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.Write(value);
        }

        public void Write(string format, params object[] args)
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.Write(format, args);
        }

        public void WriteLine()
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.WriteLine();
        }

        public void WriteLine(string value)
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.WriteLine(value);
        }

        public void WriteLine(object value)
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.WriteLine(value);
        }

        public void WriteLine(string format, params object[] args)
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.WriteLine(format, args);
        }

        public void Clear()
        {
            if (_isLocked) throw new InvalidOperationException(LockErrorMessage);
            _console.Clear();
        }
    }
}
