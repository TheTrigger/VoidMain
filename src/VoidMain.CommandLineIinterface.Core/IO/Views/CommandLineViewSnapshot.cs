using System;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public struct CommandLineViewSnapshot : IEquatable<CommandLineViewSnapshot>
    {
        public string LineContent { get; }
        public int CursorPosition { get; }

        public CommandLineViewSnapshot(ICommandLineView lineView)
        {
            LineContent = lineView.ToString();
            CursorPosition = lineView.Position;
        }

        public CommandLineViewSnapshot(string lineContent, int cursorPosition)
        {
            LineContent = lineContent;
            CursorPosition = cursorPosition;
        }

        public void ApplyTo(ICommandLineView lineView)
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
            return (obj is CommandLineViewSnapshot snapshot && Equals(snapshot));
        }

        public bool Equals(CommandLineViewSnapshot other)
        {
            return LineContent == other.LineContent
                && CursorPosition == other.CursorPosition;
        }

        public static bool operator ==(CommandLineViewSnapshot left, CommandLineViewSnapshot right) => left.Equals(right);
        public static bool operator !=(CommandLineViewSnapshot left, CommandLineViewSnapshot right) => !left.Equals(right);
    }
}
