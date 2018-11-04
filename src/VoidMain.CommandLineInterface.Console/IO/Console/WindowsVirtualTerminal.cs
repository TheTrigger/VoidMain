using System;
using System.Runtime.InteropServices;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public static class WindowsVirtualTerminal
    {
        public static bool TryEnable() => TryChangeMode(true);
        public static bool TryDisable() => TryChangeMode(false);

        private static bool TryChangeMode(bool enabled)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return false;
            }

            var handle = GetStdHandle(STD_OUTPUT_HANDLE);
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            if (!GetConsoleMode(handle, out uint mode))
            {
                return false;
            }

            if (enabled)
            {
                mode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            }
            else
            {
                mode &= ~ENABLE_VIRTUAL_TERMINAL_PROCESSING;
            }

            return SetConsoleMode(handle, mode);
        }

        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

#pragma warning disable PC003 // Native API not available in UWP
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int stdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr consoleHandle, out uint mode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr consoleHandle, uint mode);
#pragma warning restore PC003 // Native API not available in UWP
    }
}
