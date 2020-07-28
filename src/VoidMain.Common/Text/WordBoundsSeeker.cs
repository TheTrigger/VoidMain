using System;

namespace VoidMain.Text
{
    public class WordBoundsSeeker : ITextPositionSeeker
    {
        private const bool IsWhitespace = true;
        private const bool IsNotWhitespace = false;

        public bool TrySeekForward(ReadOnlySpan<char> text, int start, out int position)
        {
            if (start > text.Length - 2)
            {
                position = text.Length;
                return true;
            }

            position = start + 1;

            if (Char.IsWhiteSpace(text[position]))
            {
                position += SkipForwardWhile(IsWhitespace, text.Slice(position));
            }

            if (position == text.Length)
            {
                return true;
            }

            position += SkipForwardWhile(IsNotWhitespace, text.Slice(position));
            return true;
        }

        public bool TrySeekBackward(ReadOnlySpan<char> text, int start, out int position)
        {
            if (start < 2)
            {
                position = 0;
                return true;
            }

            position = start - 1;

            if (Char.IsWhiteSpace(text[position]))
            {
                position = SkipBackwardWhile(IsWhitespace, text.Slice(0, position));
            }

            if (position < 1)
            {
                position = 0;
                return true;
            }

            position = SkipBackwardWhile(IsNotWhitespace, text.Slice(0, position)) + 1;
            return true;
        }

        private static int SkipBackwardWhile(bool isWhitespace, ReadOnlySpan<char> span)
        {
            for (int pos = span.Length - 1; pos >= 0; pos--)
            {
                if (Char.IsWhiteSpace(span[pos]) != isWhitespace) return pos;
            }
            return -1;
        }

        private static int SkipForwardWhile(bool isWhitespace, ReadOnlySpan<char> span)
        {
            for (int pos = 0; pos < span.Length; pos++)
            {
                if (Char.IsWhiteSpace(span[pos]) != isWhitespace) return pos;
            }
            return span.Length;
        }
    }
}
