using System;
using System.Diagnostics;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    [DebuggerDisplay("{" + nameof(LineContent) + "}")]
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

        public override int GetHashCode()
        {
            return (LineContent == null ? 0 : LineContent.GetHashCode()) ^ CursorPosition;
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
