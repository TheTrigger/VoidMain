using System;

namespace VoidMain
{
    public readonly struct Color : IEquatable<Color>, IFormattable
    {
        private const int AlphaShift = 24;
        private const int RedShift = 16;
        private const int GreenShift = 8;
        private const int BlueShift = 0;

        public int Value { get; }

        /// <summary>
        /// Alpha
        /// </summary>
        public byte A => (byte)(Value >> AlphaShift & 0xFF);

        /// <summary>
        /// Red
        /// </summary>
        public byte R => (byte)(Value >> RedShift & 0xFF);

        /// <summary>
        /// Green
        /// </summary>
        public byte G => (byte)(Value >> GreenShift & 0xFF);

        /// <summary>
        /// Blue
        /// </summary>
        public byte B => (byte)(Value >> BlueShift & 0xFF);

        public Color(int value) => Value = value;

        public Color(byte red, byte green, byte blue)
            : this(255, red, green, blue) { }

        public Color(byte alpha, byte red, byte green, byte blue)
        {
            Value = unchecked(
                alpha << AlphaShift |
                red << RedShift |
                green << GreenShift |
                blue << BlueShift
                );
        }

        public static Color Parse(string value) => Parse(value.AsSpan());

        public static Color Parse(ReadOnlySpan<char> span)
        {
            if (TryParse(span, out Color color))
            {
                return color;
            }
            throw new FormatException("Input string was not in a correct format.");
        }

        public static bool TryParse(ReadOnlySpan<char> span, out Color color)
        {
            int i = span.Length > 0 && span[0] == '#' ? 1 : 0;
            int length = span.Length - i;

            int alpha = 255;
            int red = -1;
            int green = -1;
            int blue = -1;

            if (length == 8)
            {
                alpha = HexToDec(span[i++]) * 16 + HexToDec(span[i++]);
                length -= 2;
            }

            if (length == 6)
            {
                red = HexToDec(span[i++]) * 16 + HexToDec(span[i++]);
                green = HexToDec(span[i++]) * 16 + HexToDec(span[i++]);
                blue = HexToDec(span[i++]) * 16 + HexToDec(span[i]);
            }

            if (length == 4)
            {
                alpha = HexToDec(span[i++]);
                alpha = alpha * 16 + alpha;
                length--;
            }

            if (length == 3)
            {
                red = HexToDec(span[i++]);
                red = red * 16 + red;
                green = HexToDec(span[i++]);
                green = green * 16 + green;
                blue = HexToDec(span[i]);
                blue = blue * 16 + blue;
            }

            if (alpha < 0 || red < 0 || green < 0 || blue < 0)
            {
                color = default;
                return false;
            }

            color = new Color((byte)alpha, (byte)red, (byte)green, (byte)blue);
            return true;
        }

        private static int HexToDec(char hex)
        {
            if ('0' <= hex && hex <= '9') return hex - '0';
            if ('a' <= hex && hex <= 'f') return hex - 'a' + 10;
            if ('A' <= hex && hex <= 'F') return hex - 'A' + 10;
            return -1;
        }

        public override string ToString() => ToString(null, null);

        /// <summary>
        /// Converts the color value of this instance to its
        /// equivalent string representation using the specified format.
        /// <para>Supported formats: argb, #argb, ARGB, #ARGB, rgb, #rgb, RGB, #RGB.</para>
        /// </summary>
        public string ToString(string? format, IFormatProvider? formatProvider = null)
        {
            Span<char> destination = stackalloc char[9];
            if (TryFormat(destination, out int charsWritten, format, formatProvider))
            {
                return new string(destination.Slice(0, charsWritten));
            }
            throw new FormatException("Unknown format.");
        }

        public bool TryFormat(
            Span<char> destination,
            out int charsWritten,
            ReadOnlySpan<char> format = default,
            IFormatProvider? provider = null)
        {
            charsWritten = 0;

            if (destination.IsEmpty)
            {
                return false;
            }

            if (format.IsEmpty)
            {
                format = "#ARGB";
            }

            if (format[0] == '#')
            {
                destination[0] = '#';
                charsWritten = 1;
                destination = destination.Slice(1);
                format = format.Slice(1);
            }

            int value = Value;
            ReadOnlySpan<char> valueFormat;

            if (format.Equals("ARGB", StringComparison.OrdinalIgnoreCase))
            {
                valueFormat = format[0] == 'A' ? "X8" : "x8";
                charsWritten += 8;
            }
            else if (format.Equals("RGB", StringComparison.OrdinalIgnoreCase))
            {
                valueFormat = format[0] == 'R' ? "X6" : "x6";
                value = (Value << 8) / 256;
                charsWritten += 6;
            }
            else
            {
                return false;
            }

            return value.TryFormat(destination, out _, valueFormat, provider);
        }

        public override int GetHashCode() => Value;

        public override bool Equals(object? obj) => obj is Color c && Equals(c);

        public bool Equals(Color other) => Value == other.Value;

        public static bool operator ==(Color a, Color b) => a.Equals(b);

        public static bool operator !=(Color a, Color b) => !a.Equals(b);
    }
}
