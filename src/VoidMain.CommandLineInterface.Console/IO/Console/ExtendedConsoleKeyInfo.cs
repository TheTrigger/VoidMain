using System;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public struct ExtendedConsoleKeyInfo
    {
        public ConsoleKeyInfo KeyInfo { get; }
        public bool IsNextKeyAvailable { get; }

        public ExtendedConsoleKeyInfo(ConsoleKeyInfo keyInfo, bool isNextKeyAvailable)
        {
            KeyInfo = keyInfo;
            IsNextKeyAvailable = isNextKeyAvailable;
        }
    }
}
