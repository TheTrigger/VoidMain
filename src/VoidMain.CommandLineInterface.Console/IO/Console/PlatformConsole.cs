using System;
using System.IO;
using SysConsole = System.Console;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public sealed class SystemConsole : IConsole
    {
        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => SysConsole.CancelKeyPress += value;
            remove => SysConsole.CancelKeyPress -= value;
        }

        public TextReader Input
        {
            get => SysConsole.In;
            set => SysConsole.SetIn(value);
        }

        public TextWriter Output
        {
            get => SysConsole.Out;
            set => SysConsole.SetOut(value);
        }

        public TextWriter Error
        {
            get => SysConsole.Error;
            set => SysConsole.SetError(value);
        }

        public bool IsInputRedirected => SysConsole.IsInputRedirected;
        public bool IsOutputRedirected => SysConsole.IsOutputRedirected;
        public bool IsErrorRedirected => SysConsole.IsErrorRedirected;

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

        public bool IsCursorVisible
        {
#pragma warning disable PC001 // API not supported on all platforms
            get => SysConsole.CursorVisible;
#pragma warning restore PC001 // API not supported on all platforms
            set => SysConsole.CursorVisible = value;
        }

        public void SetCursorPosition(int top, int left) => SysConsole.SetCursorPosition(left, top);

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

        public bool IsKeyAvailable => SysConsole.KeyAvailable;
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
