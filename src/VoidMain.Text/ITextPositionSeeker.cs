using System;

namespace VoidMain.Text
{
    public interface ITextPositionSeeker
    {
        bool TrySeekForward(ReadOnlySpan<char> text, int start, out int position);
        bool TrySeekBackward(ReadOnlySpan<char> text, int start, out int position);
    }
}
