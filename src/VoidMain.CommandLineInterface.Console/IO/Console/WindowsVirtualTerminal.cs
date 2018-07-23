using System;
using System.Runtime.InteropServices;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public static class WindowsVirtualTerminal
    {
        public static bool TryEnable()
        {
            var stdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (stdOut == IntPtr.Zero)
            {
                return false;
            }

            if (!GetConsoleMode(stdOut, out uint outMode))
            {
                return false;
            }

            outMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            return SetConsoleMode(stdOut, outMode);
        }

        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int stdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr consoleHandle, out uint mode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr consoleHandle, uint mode);
    }
}
