using System;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public struct LineViewSnapshot : IEquatable<LineViewSnapshot>
    {
        public string LineContent { get; }
        public int CursorPosition { get; }

        public LineViewSnapshot(ILineView lineView)
        {
            LineContent = lineView.ToString();
            CursorPosition = lineView.Position;
        }

        public LineViewSnapshot(string lineContent, int cursorPosition)
        {
            LineContent = lineContent;
            CursorPosition = cursorPosition;
        }

        public void ApplyTo(ILineView lineView)
        {
            lineView.ReplaceWith(LineContent);
            lineView.MoveTo(CursorPosition);
        }

        public override string ToString() => LineContent;

        public override int GetHashCode()
        {
            return (LineContent?.GetHashCode() ?? 0) ^ CursorPosition;
        }

        public override bool Equals(object obj)
        {
            return (obj is LineViewSnapshot snapshot && Equals(snapshot));
        }

        public bool Equals(LineViewSnapshot other)
        {
            return LineContent == other.LineContent
                && CursorPosition == other.CursorPosition;
        }

        public static bool operator ==(LineViewSnapshot left, LineViewSnapshot right) => left.Equals(right);
        public static bool operator !=(LineViewSnapshot left, LineViewSnapshot right) => !left.Equals(right);
    }
}
