using System;

namespace VoidMain.CommandLineIinterface.Highlighting
{
    public class TextStyle : IEquatable<TextStyle>
    {
        public static TextStyle Default { get; } = new TextStyle(null);

        public Color Foreground { get; }
        public Color Background { get; }

        public TextStyle(Color foreground, Color background = null)
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
            return (obj is TextStyle style && Equals(style));
        }

        public bool Equals(TextStyle other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;
            return Foreground == other.Foreground
                && Background == other.Background;
        }

        public static bool operator ==(TextStyle left, TextStyle right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(TextStyle left, TextStyle right)
        {
            return !(left == right);
        }
    }
}
