using System;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public class NearestConsoleColorConverter : IConsoleColorConverter
    {
        private readonly Color[] _colors =
        {
            Colors.Black,
            Colors.DarkRed,
            Colors.DarkGreen,
            Colors.DarkBlue,
            Colors.DarkYellow,
            Colors.DarkMagenta,
            Colors.DarkCyan,
            Colors.DarkGray,
            Colors.Gray,
            Colors.Red,
            Colors.Green,
            Colors.Blue,
            Colors.Yellow,
            Colors.Magenta,
            Colors.Cyan,
            Colors.White
        };

        private readonly SupportedConsoleColorConverter _supportedConverter = new SupportedConsoleColorConverter();

        public bool TryConvert(Color color, out ConsoleColor consoleColor)
        {
            if (_supportedConverter.TryConvert(color, out consoleColor))
            {
                return true;
            }

            var nearest = Colors.White;
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
