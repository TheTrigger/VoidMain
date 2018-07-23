using System;
using System.IO;
using SysConsole = System.Console;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public sealed class PlatformConsole : IConsole
    {
        private PlatformConsole() { }

        private static readonly Lazy<PlatformConsole> _instance
            = new Lazy<PlatformConsole>(() => new PlatformConsole());

        public static PlatformConsole Instance => _instance.Value;

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => SysConsole.CancelKeyPress += value;
            remove => SysConsole.CancelKeyPress -= value;
        }

        public TextReader In
        {
            get => SysConsole.In;
            set => SysConsole.SetIn(value);
        }

        public TextWriter Out
        {
            get => SysConsole.Out;
            set => SysConsole.SetOut(value);
        }

#pragma warning disable PC001 // API not supported on all platforms
        public string Title
        {
            get => SysConsole.Title;
            set => SysConsole.Title = value;
        }

        public int BufferHeight
        {
            get => SysConsole.BufferHeight;
            set => SysConsole.BufferHeight = value;
        }

        public int BufferWidth
        {
            get => SysConsole.BufferWidth;
            set => SysConsole.BufferWidth = value;
        }
#pragma warning restore PC001 // API not supported on all platforms

        public int CursorTop
        {
            get => SysConsole.CursorTop;
            set => SysConsole.CursorTop = value;
        }

        public int CursorLeft
        {
            get => SysConsole.CursorLeft;
            set => SysConsole.CursorLeft = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => SysConsole.BackgroundColor;
            set => SysConsole.BackgroundColor = value;
        }

        public ConsoleColor ForegroundColor
        {
            get => SysConsole.ForegroundColor;
            set => SysConsole.ForegroundColor = value;
        }

        public void ResetColors() => SysConsole.ResetColor();

        public bool KeyAvailable => SysConsole.KeyAvailable;
        public ConsoleKeyInfo ReadKey(bool intercept) => SysConsole.ReadKey(intercept);
        public string ReadLine() => SysConsole.ReadLine();

        public void Write(char value) => SysConsole.Write(value);
        public void Write(char value, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SysConsole.Write(value);
            }
        }
        public void Write(string value) => SysConsole.Write(value);
        public void Write(object value) => SysConsole.Write(value);
        public void Write(string format, params object[] args) => SysConsole.Write(format, args);

        public void WriteLine() => SysConsole.WriteLine();
        public void WriteLine(string value) => SysConsole.WriteLine(value);
        public void WriteLine(object value) => SysConsole.WriteLine(value);
        public void WriteLine(string format, params object[] args) => SysConsole.WriteLine(format, args);

        public void Clear() => SysConsole.Clear();
    }
}
