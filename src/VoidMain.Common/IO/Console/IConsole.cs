using System;
using System.IO;

namespace VoidMain.IO.Console
{
    public interface IConsole
    {
        event ConsoleCancelEventHandler CancelKeyPress;

        TextReader Input { get; set; }
        TextWriter Output { get; set; }
        TextWriter Error { get; set; }

        bool IsInputRedirected { get; }
        bool IsOutputRedirected { get; }
        bool IsErrorRedirected { get; }

        string Title { get; set; }

        int BufferHeight { get; set; }
        int BufferWidth { get; set; }

        int CursorTop { get; set; }
        int CursorLeft { get; set; }
        bool IsCursorVisible { get; set; }
        void SetCursorPosition(int top, int left);

        ConsoleColor BackgroundColor { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        void ResetColors();

        bool IsKeyAvailable { get; }
        ConsoleKeyInfo ReadKey(bool intercept);
        string ReadLine();

        void Write(char value);
        void Write(char value, int count);
        void Write(string? value);
        void Write(object? value);
        void Write(ReadOnlySpan<char> value);
        void Write(string format, params object[] args);

        void WriteLine();
        void WriteLine(string? value);
        void WriteLine(object? value);
        void WriteLine(ReadOnlySpan<char> value);
        void WriteLine(string format, params object[] args);

        void Clear();
    }
}
