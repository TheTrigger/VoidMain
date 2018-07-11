using System;
using System.IO;

namespace VoidMain.CommandLineIinterface.IO.Console
{
    public interface IConsole
    {
        event ConsoleCancelEventHandler CancelKeyPress;

        TextReader In { get; set; }
        TextWriter Out { get; set; }

        int BufferHeight { get; set; }
        int BufferWidth { get; set; }
        int CursorTop { get; set; }
        int CursorLeft { get; set; }

        ConsoleColor BackgroundColor { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        void ResetColors();

        bool KeyAvailable { get; }
        ConsoleKeyInfo ReadKey(bool intercept);
        string ReadLine();

        void Write(char value);
        void Write(char value, int count);
        void Write(string value);
        void Write(object value);
        void Write(string format, params object[] args);

        void WriteLine();
        void WriteLine(string value);
        void WriteLine(object value);
        void WriteLine(string format, params object[] args);

        void Clear();
    }
}
