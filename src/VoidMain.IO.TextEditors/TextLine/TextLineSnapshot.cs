using System;

namespace VoidMain.IO.TextEditors.TextLine
{
    public readonly struct TextLineSnapshot : IEquatable<TextLineSnapshot>
    {
        public string Content { get; }
        public int CursorPosition { get; }

        public TextLineSnapshot(string content, int cursorPosition)
        {
            Content = content;
            CursorPosition = cursorPosition;
        }

        public TextLineSnapshot(ITextLine textLine)
        {
            Content = new string(textLine.Span);
            CursorPosition = textLine.Position;
        }

        public TextLineSnapshot WithNewPosition(int cursorPosition)
        {
            return new TextLineSnapshot(Content, cursorPosition);
        }

        public void ApplyTo(ITextLine textLine)
        {
            textLine.Position = 0;
            textLine.TypeOver(Content);
            textLine.Delete(textLine.Length - textLine.Position);
            textLine.Position = CursorPosition;
        }

        public override string ToString() => Content;

        public override int GetHashCode()
            => HashCode.Combine(Content, CursorPosition);

        public override bool Equals(object? obj)
            => obj is TextLineSnapshot snapshot && Equals(snapshot);

        public bool Equals(TextLineSnapshot other)
            => Content == other.Content && CursorPosition == other.CursorPosition;
    }
}
