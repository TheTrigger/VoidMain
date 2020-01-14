using System;

namespace VoidMain.Text
{
    public interface ITextWriter
    {
        void Write(char value);
        void Write(char value, int count);
        void Write(string value);
        void Write(ReadOnlySpan<char> value);
        void WriteNewLine();
    }
}
