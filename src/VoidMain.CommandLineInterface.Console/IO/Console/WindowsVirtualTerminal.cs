using System;
using System.Runtime.InteropServices;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public static class WindowsVirtualTerminal
    {
        public static bool IsEnabled()
        {
            if (!TryGetHandle(out var handle)) return false;
            if (!GetConsoleMode(handle, out uint mode)) return false;
            return (mode & ENABLE_VIRTUAL_TERMINAL_PROCESSING) == ENABLE_VIRTUAL_TERMINAL_PROCESSING;
        }

        public static bool TryEnable() => TrySetEnabled(true);
        public static bool TryDisable() => TrySetEnabled(false);

        public static bool TrySetEnabled(bool enabled)
        {
            if (!TryGetHandle(out var handle)) return false;
            if (!GetConsoleMode(handle, out uint mode)) return false;

            mode = enabled
                ? mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING
                : mode & ~ENABLE_VIRTUAL_TERMINAL_PROCESSING;

            return SetConsoleMode(handle, mode);
        }

        private static bool TryGetHandle(out IntPtr handle)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                handle = IntPtr.Zero;
                return false;
            }

            handle = GetStdHandle(STD_OUTPUT_HANDLE);
            return handle != IntPtr.Zero;
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
