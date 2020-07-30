using System;

namespace VoidMain.Text.Style
{
    public readonly struct TextStyle : IEquatable<TextStyle>
    {
        public Color? Foreground { get; }
        public Color? Background { get; }

        public TextStyle(
            Color? foreground = default,
            Color? background = default)
        {
            Foreground = foreground;
            Background = background;
        }

        public static readonly TextStyle Default = default;

        public override string ToString()
        {
            string f = Foreground?.ToString() ?? " --- ";
            string b = Background?.ToString() ?? " --- ";
            return $"{f}/{b}";
        }

        public override int GetHashCode()
            => HashCode.Combine(Foreground, Background);

        public override bool Equals(object? obj)
            => obj is TextStyle style && Equals(style);

        public bool Equals(TextStyle other)
            => Foreground == other.Foreground && Background == other.Background;

        public static bool operator ==(in TextStyle a, in TextStyle b) => a.Equals(b);

        public static bool operator !=(in TextStyle a, in TextStyle b) => !a.Equals(b);

        public static implicit operator TextStyle(Color foreground) => new TextStyle(foreground);
    }
}
