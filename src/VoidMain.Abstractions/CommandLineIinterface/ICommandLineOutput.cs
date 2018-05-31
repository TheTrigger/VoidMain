﻿using System;

namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineOutput
    {
        void Write(char value);
        void Write(string value);
        void Write(object value);
        void Write(string format, params object[] args);
        void Write(IFormatProvider formatProvider, string format, params object[] args);

        void WriteLine();
        void WriteLine(string value);
        void WriteLine(object value);
        void WriteLine(string format, params object[] args);
        void WriteLine(IFormatProvider formatProvider, string format, params object[] args);

        void Write(Color foreground, char value);
        void Write(Color foreground, string value);
        void Write(Color foreground, object value);

        void WriteLine(Color foreground, string value);
        void WriteLine(Color foreground, object value);

        void Write(Color foreground, Color background, char value);
        void Write(Color foreground, Color background, string value);
        void Write(Color foreground, Color background, object value);

        void WriteLine(Color foreground, Color background, string value);
        void WriteLine(Color foreground, Color background, object value);

        void Write(ColoredFormat format);
        void WriteLine(ColoredFormat format);

        void Clear();
    }
}
