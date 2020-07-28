using System;
using System.Text;

namespace VoidMain.Text
{
    public class StringBuilderWriter : ITextWriter
    {
        private readonly StringBuilder _stringBuilder;

        public int Length => _stringBuilder.Length;

        public int Capacity => _stringBuilder.Capacity;

        public StringBuilderWriter()
            => _stringBuilder = new StringBuilder();

        public StringBuilderWriter(int capacity)
            => _stringBuilder = new StringBuilder(capacity);

        public StringBuilderWriter(StringBuilder stringBuilder)
            => _stringBuilder = stringBuilder;

        public void Write(char value) => _stringBuilder.Append(value);

        public void Write(char value, int count) => _stringBuilder.Append(value, count);

        public void Write(string? value) => _stringBuilder.Append(value);

        public void Write(ReadOnlySpan<char> value) => _stringBuilder.Append(value);

        public void WriteNewLine() => _stringBuilder.AppendLine();

        public void Clear() => _stringBuilder.Clear();

        public override string ToString() => _stringBuilder.ToString();
    }
}
