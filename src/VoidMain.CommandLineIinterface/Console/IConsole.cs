﻿using System;
using System.IO;

namespace VoidMain.CommandLineIinterface.Console
{
    public interface IConsole
    {
        TextReader In { get; set; }
        TextWriter Out { get; set; }
        event ConsoleCancelEventHandler CancelKeyPress;

        int BufferHeight { get; set; }
        int BufferWidth { get; set; }
        int CursorTop { get; set; }
        int CursorLeft { get; set; }

        bool KeyAvailable { get; }
        ConsoleKeyInfo ReadKey(bool intercept);
        string ReadLine();

        void Write(char value);
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
