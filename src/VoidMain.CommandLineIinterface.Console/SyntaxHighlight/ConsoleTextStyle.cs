using System;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class ConsoleTextStyle : IEquatable<ConsoleTextStyle>
    {
        public static ConsoleTextStyle Default { get; } = new ConsoleTextStyle(null);

        public Color Foreground { get; }
        public Color Background { get; }

        public ConsoleTextStyle(Color foreground, Color background = null)
        {
            Foreground = foreground;
            Background = background;
        }

        public override string ToString()
        {
            return Background == null
                ? $"{{{Foreground}}}"
                : $"{{{Foreground}, {Background}}}";
        }

        public override int GetHashCode()
        {
            return (Foreground?.GetHashCode() ?? 0)
                 ^ (Background?.GetHashCode() ?? 0);
        }

        public override bool Equals(object obj)
        {
            return (obj is ConsoleTextStyle style && Equals(style));
        }

        public bool Equals(ConsoleTextStyle other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;
            return Foreground == other.Foreground
                && Background == other.Background;
        }

        public static bool operator ==(ConsoleTextStyle left, ConsoleTextStyle right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(ConsoleTextStyle left, ConsoleTextStyle right)
        {
            return !(left == right);
        }
    }
}
