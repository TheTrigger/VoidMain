using System;
using System.IO;

namespace VoidMain.CommandLineIinterface.IO.Console.Internal
{
    public sealed class PlatformConsole : IConsole
    {
        private PlatformConsole() { }

        private static readonly Lazy<PlatformConsole> _instance
            = new Lazy<PlatformConsole>(() => new PlatformConsole());

        public static PlatformConsole Instance => _instance.Value;

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => System.Console.CancelKeyPress += value;
            remove => System.Console.CancelKeyPress -= value;
        }

        public TextReader In
        {
            get => System.Console.In;
            set => System.Console.SetIn(value);
        }

        public TextWriter Out
        {
            get => System.Console.Out;
            set => System.Console.SetOut(value);
        }

#pragma warning disable PC001 // API not supported on all platforms
        public int BufferHeight
        {
            get => System.Console.BufferHeight;
            set => System.Console.BufferHeight = value;
        }

        public int BufferWidth
        {
            get => System.Console.BufferWidth;
            set => System.Console.BufferWidth = value;
        }
#pragma warning restore PC001 // API not supported on all platforms

        public int CursorTop
        {
            get => System.Console.CursorTop;
            set => System.Console.CursorTop = value;
        }

        public int CursorLeft
        {
            get => System.Console.CursorLeft;
            set => System.Console.CursorLeft = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => System.Console.BackgroundColor;
            set => System.Console.BackgroundColor = value;
        }

        public ConsoleColor ForegroundColor
        {
            get => System.Console.ForegroundColor;
            set => System.Console.ForegroundColor = value;
        }

        public void ResetColors() => System.Console.ResetColor();

        public bool KeyAvailable => System.Console.KeyAvailable;
        public ConsoleKeyInfo ReadKey(bool intercept) => System.Console.ReadKey(intercept);
        public string ReadLine() => System.Console.ReadLine();

        public void Write(char value) => System.Console.Write(value);
        public void Write(string value) => System.Console.Write(value);
        public void Write(object value) => System.Console.Write(value);
        public void Write(string format, params object[] args) => System.Console.Write(format, args);

        public void WriteLine() => System.Console.WriteLine();
        public void WriteLine(string value) => System.Console.WriteLine(value);
        public void WriteLine(object value) => System.Console.WriteLine(value);
        public void WriteLine(string format, params object[] args) => System.Console.WriteLine(format, args);

        public void Clear() => System.Console.Clear();
    }
}
