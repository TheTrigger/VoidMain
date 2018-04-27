using System;

namespace VoidMain.CommandLineIinterface
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

        private readonly long _value;

        public byte A => (byte)((_value >> AlphaShift) & 0xFF);
        public byte R => (byte)((_value >> RedShift) & 0xFF);
        public byte G => (byte)((_value >> GreenShift) & 0xFF);
        public byte B => (byte)((_value >> BlueShift) & 0xFF);

        private Color() { }

        public Color(byte red, byte green, byte blue)
        {
            _value = MakeArgb(255, red, green, blue);
        }

        private static long MakeArgb(byte alpha, byte red, byte green, byte blue)
        {
            return (long)(unchecked((uint)(
                red << RedShift |
                green << GreenShift |
                blue << BlueShift |
                alpha << AlphaShift
                ))) & 0xffffffff;
        }

        public bool Equals(Color other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(other, this)) return true;
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Color);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return "#" + _value.ToString("X8");
        }
    }
}
