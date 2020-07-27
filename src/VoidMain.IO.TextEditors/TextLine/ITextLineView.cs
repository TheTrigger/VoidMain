using System;

namespace VoidMain.IO.TextEditors.TextLine
{
    public interface ITextLineView : ITextLine
    {
        void ClearState();
        void SetState(ReadOnlySpan<char> text, int position);
        void Render();
        void EndLine();
    }
}
