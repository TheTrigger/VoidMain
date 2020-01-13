using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public class SupportedConsoleColorConverter : IConsoleColorConverter
    {
        private readonly Dictionary<Color, ConsoleColor> _colors = new Dictionary<Color, ConsoleColor>
        {
            [Colors.Black] = ConsoleColor.Black,
            [Colors.DarkRed] = ConsoleColor.DarkRed,
            [Colors.DarkGreen] = ConsoleColor.DarkGreen,
            [Colors.DarkBlue] = ConsoleColor.DarkBlue,
            [Colors.DarkYellow] = ConsoleColor.DarkYellow,
            [Colors.DarkMagenta] = ConsoleColor.DarkMagenta,
            [Colors.DarkCyan] = ConsoleColor.DarkCyan,
            [Colors.DarkGray] = ConsoleColor.DarkGray,
            [Colors.Gray] = ConsoleColor.Gray,
            [Colors.Red] = ConsoleColor.Red,
            [Colors.Green] = ConsoleColor.Green,
            [Colors.Blue] = ConsoleColor.Blue,
            [Colors.Yellow] = ConsoleColor.Yellow,
            [Colors.Magenta] = ConsoleColor.Magenta,
            [Colors.Cyan] = ConsoleColor.Cyan,
            [Colors.White] = ConsoleColor.White
        };

        public bool TryConvert(Color color, out ConsoleColor consoleColor)
        {
            if (color == null)
            {
                throw new ArgumentNullException(nameof(color));
            }
            return _colors.TryGetValue(color, out consoleColor);
        }
    }
}
