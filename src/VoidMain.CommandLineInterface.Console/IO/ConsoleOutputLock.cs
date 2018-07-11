using System;

namespace VoidMain.CommandLineInterface.IO
{
    public class ConsoleOutputLock
    {
        public bool IsLocked { get; private set; }

        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;

        public void ThrowIfLocked()
        {
            if (IsLocked)
            {
                throw new InvalidOperationException("Output is locked during read operation.");
            }
        }
    }
}
