using System;
using System.IO;

namespace VoidMain.CommandLineInterface.IO.Console.Tests
{
    public class MockConsole : IConsole
    {
        public virtual event ConsoleCancelEventHandler CancelKeyPress;

        public virtual TextReader Input { get; set; }
        public virtual TextWriter Output { get; set; }
        public virtual TextWriter Error { get; set; }

        public virtual bool IsInputRedirected { get; }
        public virtual bool IsOutputRedirected { get; }
        public virtual bool IsErrorRedirected { get; }

        public virtual string Title { get; set; }

        public virtual int BufferHeight { get; set; }
        public virtual int BufferWidth { get; set; }

        public virtual int CursorTop { get; set; }
        public virtual int CursorLeft { get; set; }
        public virtual bool IsCursorVisible { get; set; }

        public virtual void SetCursorPosition(int top, int left)
        {
            CursorTop = top;
            CursorLeft = left;
        }

        public virtual ConsoleColor BackgroundColor { get; set; }
        public virtual ConsoleColor ForegroundColor { get; set; }

        public virtual void ResetColors()
        {
            ForegroundColor = default;
            BackgroundColor = default;
        }

        public virtual bool IsKeyAvailable { get; }
        public virtual ConsoleKeyInfo ReadKey(bool intercept) => default;
        public virtual string ReadLine() => default;

        public virtual void Write(char value) { }
        public virtual void Write(char value, int count) { }
        public virtual void Write(string value) { }
        public virtual void Write(object value) { }
        public virtual void Write(string format, params object[] args) { }

        public virtual void WriteLine() { }
        public virtual void WriteLine(string value) { }
        public virtual void WriteLine(object value) { }
        public virtual void WriteLine(string format, params object[] args) { }

        public virtual void Clear() { }
    }
}
