using System;
using System.Buffers;
using System.IO;

namespace VoidMain.Text
{
    public class TextWriterWrapper : ITextWriter, IDisposable
    {
        private readonly TextWriter _textWriter;

        public TextWriterWrapper(TextWriter textWriter)
            => _textWriter = textWriter;

        public void Write(char value) => _textWriter.Write(value);

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
            _textWriter.Write(buffer);
        }

        private void WriteSlow(char value, int count)
        {
            char[] array = ArrayPool<char>.Shared.Rent(count);
            Array.Fill(array, value, 0, count);
            _textWriter.Write(array, 0, count);
            ArrayPool<char>.Shared.Return(array);
        }

        public void Write(string value) => _textWriter.Write(value);

        public void Write(ReadOnlySpan<char> value) => _textWriter.Write(value);

        public void WriteNewLine() => _textWriter.WriteLine();

        public void Flush() => _textWriter.Flush();

        public void Dispose() => _textWriter.Dispose();
    }
}
