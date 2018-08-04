using System;

namespace VoidMain.CommandLineInterface.IO.Views
{
    public struct LineViewSnapshot : IEquatable<LineViewSnapshot>
    {
        public string Content { get; }
        public int Position { get; }

        public LineViewSnapshot(ILineView lineView)
        {
            Content = lineView.ToString();
            Position = lineView.Position;
        }

        public LineViewSnapshot(string content, int position)
        {
            Content = content;
            Position = position;
        }

        public void ApplyTo(ILineView lineView)
        {
            lineView.ReplaceWith(Content);
            lineView.MoveTo(Position);
        }

        public override string ToString() => Content;

        public override int GetHashCode()
        {
            return (Content?.GetHashCode() ?? 0) ^ Position;
        }

        public override bool Equals(object obj)
        {
            return (obj is LineViewSnapshot snapshot && Equals(snapshot));
        }

        public bool Equals(LineViewSnapshot other)
        {
            return Content == other.Content
                && Position == other.Position;
        }

        public static bool operator ==(LineViewSnapshot left, LineViewSnapshot right) => left.Equals(right);
        public static bool operator !=(LineViewSnapshot left, LineViewSnapshot right) => !left.Equals(right);
    }
}
