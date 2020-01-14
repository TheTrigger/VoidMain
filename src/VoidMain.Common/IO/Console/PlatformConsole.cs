using System;
using System.Buffers;
using System.IO;
using C = System.Console;

namespace VoidMain.IO.Console
{
    public sealed class SystemConsole : IConsole
    {
        public event ConsoleCancelEventHandler CancelKeyPress
        {
            add => C.CancelKeyPress += value;
            remove => C.CancelKeyPress -= value;
        }

        public TextReader Input
        {
            get => C.In;
            set => C.SetIn(value);
        }

        public TextWriter Output
        {
            get => C.Out;
            set => C.SetOut(value);
        }

        public TextWriter Error
        {
            get => C.Error;
            set => C.SetError(value);
        }

        public bool IsInputRedirected => C.IsInputRedirected;

        public bool IsOutputRedirected => C.IsOutputRedirected;

        public bool IsErrorRedirected => C.IsErrorRedirected;

        public string Title
        {
            get => C.Title;
            set => C.Title = value;
        }

        public int BufferHeight
        {
            get => C.BufferHeight;
            set => C.BufferHeight = value;
        }

        public int BufferWidth
        {
            get => C.BufferWidth;
            set => C.BufferWidth = value;
        }

        public int CursorTop
        {
            get => C.CursorTop;
            set => C.CursorTop = value;
        }

        public int CursorLeft
        {
            get => C.CursorLeft;
            set => C.CursorLeft = value;
        }

        public bool IsCursorVisible
        {
            get => C.CursorVisible;
            set => C.CursorVisible = value;
        }

        public void SetCursorPosition(int top, int left)
            => C.SetCursorPosition(left, top);

        public ConsoleColor BackgroundColor
        {
            get => C.BackgroundColor;
            set => C.BackgroundColor = value;
        }

        public ConsoleColor ForegroundColor
        {
            get => C.ForegroundColor;
            set => C.ForegroundColor = value;
        }

        public void ResetColors() => C.ResetColor();

        public bool IsKeyAvailable => C.KeyAvailable;

        public ConsoleKeyInfo ReadKey(bool intercept)
            => C.ReadKey(intercept);

        public string ReadLine() => C.ReadLine();

        public void Write(char value) => C.Write(value);

        public void Write(char value, int count)
        {
            if (count == 0) return;

            if (count > 128)
            {
                WriteSlow(value, count);
                return;
            }

            Span<char> buffer = stackalloc char[count];
            buffer.Fill(value);
            C.Out.Write(buffer);
        }

        private static void WriteSlow(char value, int count)
        {
            char[] array = ArrayPool<char>.Shared.Rent(count);
            Array.Fill(array, value, 0, count);
            C.Out.Write(array, 0, count);
            ArrayPool<char>.Shared.Return(array);
        }

        public void Write(string value) => C.Write(value);

        public void Write(object value) => C.Write(value);

        public void Write(ReadOnlySpan<char> value) => C.Out.Write(value);

        public void Write(string format, params object[] args) => C.Write(format, args);

        public void WriteLine() => C.WriteLine();

        public void WriteLine(string value) => C.WriteLine(value);

        public void WriteLine(object value) => C.WriteLine(value);

        public void WriteLine(ReadOnlySpan<char> value) => C.Out.WriteLine(value);

        public void WriteLine(string format, params object[] args) => C.WriteLine(format, args);

        public void Clear() => C.Clear();
    }
}
