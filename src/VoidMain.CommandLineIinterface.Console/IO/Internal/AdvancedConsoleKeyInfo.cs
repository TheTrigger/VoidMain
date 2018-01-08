﻿using System;

namespace VoidMain.CommandLineIinterface.IO.Console.Internal
{
    public struct AdvancedConsoleKeyInfo
    {
        public ConsoleKeyInfo KeyInfo { get; }
        public bool IsNextKeyAvailable { get; }

        public AdvancedConsoleKeyInfo(ConsoleKeyInfo keyInfo, bool isNextKeyAvailable)
        {
            KeyInfo = keyInfo;
            IsNextKeyAvailable = isNextKeyAvailable;
        }
    }
}