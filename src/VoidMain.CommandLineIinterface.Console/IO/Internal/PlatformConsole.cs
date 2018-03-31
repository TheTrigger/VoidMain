using System;
using System.IO;

namespace VoidMain.CommandLineIinterface.IO.Internal
{
    public sealed class PlatformConsole : IConsole
    {
        private PlatformConsole() { }

        private static readonly Lazy<PlatformConsole> _instance
            = new Lazy<PlatformConsole>(() => new PlatformConsole());

        public static PlatformConsole Instance => _instance.Value;

        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => Console.CancelKeyPress += value;
            remove => Console.CancelKeyPress -= value;
        }

        public TextReader In
        {
            get => Console.In;
            set => Console.SetIn(value);
        }

        public TextWriter Out
        {
            get => Console.Out;
            set => Console.SetOut(value);
        }

#pragma warning disable PC001 // API not supported on all platforms
        public int BufferHeight
        {
            get => Console.BufferHeight;
            set => Console.BufferHeight = value;
        }

        public int BufferWidth
        {
            get => Console.BufferWidth;
            set => Console.BufferWidth = value;
        }
#pragma warning restore PC001 // API not supported on all platforms

        public int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        public int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public void ResetColors() => Console.ResetColor();

        public bool KeyAvailable => Console.KeyAvailable;
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
        public string ReadLine() => Console.ReadLine();

        public void Write(char value) => Console.Write(value);
        public void Write(string value) => Console.Write(value);
        public void Write(object value) => Console.Write(value);
        public void Write(string format, params object[] args) => Console.Write(format, args);

        public void WriteLine() => Console.WriteLine();
        public void WriteLine(string value) => Console.WriteLine(value);
        public void WriteLine(object value) => Console.WriteLine(value);
        public void WriteLine(string format, params object[] args) => Console.WriteLine(format, args);

        public void Clear() => Console.Clear();
    }
}
