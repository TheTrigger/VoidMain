using System;

namespace VoidMain.CommandLineIinterface.IO.Console
{
    public class NearestConsoleColorConverter : IConsoleColorConverter
    {
        private readonly Color[] _colors =
        {
            Color.Black,
            Color.DarkBlue,
            Color.DarkGreen,
            Color.DarkCyan,
            Color.DarkRed,
            Color.DarkMagenta,
            Color.DarkYellow,
            Color.Gray,
            Color.DarkGray,
            Color.Blue,
            Color.Green,
            Color.Cyan,
            Color.Red,
            Color.Magenta,
            Color.Yellow,
            Color.White
        };

        private readonly SupportedConsoleColorConverter _supportedConverter = new SupportedConsoleColorConverter();

        public bool TryConvert(Color color, out ConsoleColor consoleColor)
        {
            if (_supportedConverter.TryConvert(color, out consoleColor))
            {
                return true;
            }

            var nearest = Color.White;
            double delta = double.MaxValue;

            foreach (var c in _colors)
            {
                var d = Math.Pow(c.R - color.R, 2.0) + Math.Pow(c.G - color.G, 2.0) + Math.Pow(c.B - color.B, 2.0);
                if (d < delta)
                {
                    delta = d;
                    nearest = c;
                }
            }

            return _supportedConverter.TryConvert(nearest, out consoleColor);
        }
    }
}
