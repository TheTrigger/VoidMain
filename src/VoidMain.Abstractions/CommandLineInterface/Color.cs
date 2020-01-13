using System;
using System.Diagnostics;

namespace VoidMain.CommandLineInterface
{
    [DebuggerDisplay(@"{ToString(""argb-hex""),nq}")]
    public readonly struct Color : IEquatable<Color>, IFormattable
    {
        private const int AlphaShift = 24;
        private const int RedShift = 16;
        private const int GreenShift = 8;
        private const int BlueShift = 0;

        public uint Value { get; }

        /// <summary>
        /// Alpha
        /// </summary>
        public byte A => (byte)((Value >> AlphaShift) & 0xFF);

        /// <summary>
        /// Red
        /// </summary>
        public byte R => (byte)((Value >> RedShift) & 0xFF);

        /// <summary>
        /// Green
        /// </summary>
        public byte G => (byte)((Value >> GreenShift) & 0xFF);

        /// <summary>
        /// Blue
        /// </summary>
        public byte B => (byte)((Value >> BlueShift) & 0xFF);

        public Color(uint value) => Value = value;

        public Color(byte red, byte green, byte blue)
            : this(255, red, green, blue) { }

        public Color(byte alpha, byte red, byte green, byte blue)
        {
            Value = unchecked((uint)(
                alpha << AlphaShift |
                red << RedShift |
                green << GreenShift |
                blue << BlueShift
                ));
        }

        public static Color Parse(string value)
        {
            if (TryParse(value, out Color color))
            {
                return color;
            }
            throw new FormatException("Input string was not in a correct format.");
        }

        public static bool TryParse(string value, out Color color)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // Index of the first digit
            int i = value.Length > 0 && value[0] == '#' ? 1 : 0;
            int length = value.Length - i;

            int alpha = 255;
            int red = -1;
            int green = -1;
            int blue = -1;

            if (length == 8)
            {
                alpha = HexToDec(value[i++]) * 16 + HexToDec(value[i++]);
                length -= 2;
            }

            if (length == 6)
            {
                red = HexToDec(value[i++]) * 16 + HexToDec(value[i++]);
                green = HexToDec(value[i++]) * 16 + HexToDec(value[i++]);
                blue = HexToDec(value[i++]) * 16 + HexToDec(value[i]); // No need to increment the last one
            }

            if (length == 4)
            {
                alpha = HexToDec(value[i++]);
                alpha = alpha * 16 + alpha;
                length--;
            }

            if (length == 3)
            {
                red = HexToDec(value[i++]);
                red = red * 16 + red;
                green = HexToDec(value[i++]);
                green = green * 16 + green;
                blue = HexToDec(value[i]); // No need to increment the last one
                blue = blue * 16 + blue;
            }

            if (alpha < 0 || red < 0 || green < 0 || blue < 0)
            {
                color = new Color();
                return false;
            }

            color = new Color((byte)alpha, (byte)red, (byte)green, (byte)blue);
            return true;
        }

        private static int HexToDec(char hex)
        {
            if (hex >= '0' && hex <= '9') return hex - '0';
            if (hex >= 'a' && hex <= 'f') return hex - 'a' + 10;
            if (hex >= 'A' && hex <= 'F') return hex - 'A' + 10;
            return -1;
        }

        public override string ToString() => ToString(null, null);

        /// <summary>
        /// Converts the color value of this instance to its
        /// equivalent string representation using the specified format.
        /// <para>Supported formats: argb-dec, rgb-dec, argb-hex, rgb-hex, ARGB-HEX, RGB-HEX.</para>
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            if (String.IsNullOrEmpty(format))
            {
                format = "argb-hex";
            }

            if (comparer.Equals(format, "argb-hex"))
            {
                return Char.IsUpper(format[0])
                    ? $"#{Value:X8}"
                    : $"#{Value:x8}";
            }

            if (comparer.Equals(format, "rgb-hex"))
            {
                return Char.IsUpper(format[0])
                    ? $"#{(Value << 8) / 256:X6}"
                    : $"#{(Value << 8) / 256:x6}";
            }

            if (comparer.Equals(format, "argb-dec"))
            {
                return $"({A},{R},{G},{B})";
            }

            if (comparer.Equals(format, "rgb-dec"))
            {
                return $"({R},{G},{B})";
            }

            throw new FormatException("Input string was not in a correct format.");
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj) => obj is Color c && Equals(c);

        public bool Equals(Color other) => Value == other.Value;

        public static bool operator ==(Color a, Color b) => a.Equals(b);

        public static bool operator !=(Color a, Color b) => !a.Equals(b);
    }
}
