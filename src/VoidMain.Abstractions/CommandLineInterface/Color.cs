using System;

namespace VoidMain.CommandLineInterface
{
    public class Color : IEquatable<Color>
    {
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color DarkBlue = new Color(0, 0, 128);
        public static readonly Color DarkGreen = new Color(0, 128, 0);
        public static readonly Color DarkCyan = new Color(0, 128, 128);
        public static readonly Color DarkRed = new Color(128, 0, 0);
        public static readonly Color DarkMagenta = new Color(128, 0, 128);
        public static readonly Color DarkYellow = new Color(128, 128, 0);
        public static readonly Color Gray = new Color(192, 192, 192);
        public static readonly Color DarkGray = new Color(128, 128, 128);
        public static readonly Color Blue = new Color(0, 0, 255);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Cyan = new Color(0, 255, 255);
        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Magenta = new Color(255, 0, 255);
        public static readonly Color Yellow = new Color(255, 255, 0);
        public static readonly Color White = new Color(255, 255, 255);

        private const int AlphaShift = 24;
        private const int RedShift = 16;
        private const int GreenShift = 8;
        private const int BlueShift = 0;

        public uint Value { get; }

        public byte A => (byte)((Value >> AlphaShift) & 0xFF);
        public byte R => (byte)((Value >> RedShift) & 0xFF);
        public byte G => (byte)((Value >> GreenShift) & 0xFF);
        public byte B => (byte)((Value >> BlueShift) & 0xFF);

        private Color() { }

        public Color(byte red, byte green, byte blue)
        {
            Value = MakeArgb(255, red, green, blue);
        }

        public Color(byte alpha, byte red, byte green, byte blue)
        {
            Value = MakeArgb(alpha, red, green, blue);
        }

        public Color(uint value)
        {
            Value = value;
        }

        public string ToHexString() => Value.ToString("X8");

        private static uint MakeArgb(byte alpha, byte red, byte green, byte blue)
        {
            return (unchecked((uint)(
                red << RedShift |
                green << GreenShift |
                blue << BlueShift |
                alpha << AlphaShift
                )));
        }

        public bool Equals(Color other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj) => Equals(obj as Color);
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => "#" + ToHexString();
    }
}
