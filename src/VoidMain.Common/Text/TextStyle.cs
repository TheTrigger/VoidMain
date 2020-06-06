using System;

namespace VoidMain.Text
{
    public readonly struct TextStyle : IEquatable<TextStyle>
    {
        public Nullable<Color> Foreground { get; }
        public Nullable<Color> Background { get; }

        public TextStyle(
            Nullable<Color> foreground = default,
            Nullable<Color> background = default)
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
    }
}
