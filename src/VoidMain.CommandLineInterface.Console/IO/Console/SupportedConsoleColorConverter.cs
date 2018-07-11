using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public class SupportedConsoleColorConverter : IConsoleColorConverter
    {
        private readonly Dictionary<Color, ConsoleColor> _colors = new Dictionary<Color, ConsoleColor>
        {
            [Color.Black] = ConsoleColor.Black,
            [Color.DarkBlue] = ConsoleColor.DarkBlue,
            [Color.DarkGreen] = ConsoleColor.DarkGreen,
            [Color.DarkCyan] = ConsoleColor.DarkCyan,
            [Color.DarkRed] = ConsoleColor.DarkRed,
            [Color.DarkMagenta] = ConsoleColor.DarkMagenta,
            [Color.DarkYellow] = ConsoleColor.DarkYellow,
            [Color.Gray] = ConsoleColor.Gray,
            [Color.DarkGray] = ConsoleColor.DarkGray,
            [Color.Blue] = ConsoleColor.Blue,
            [Color.Green] = ConsoleColor.Green,
            [Color.Cyan] = ConsoleColor.Cyan,
            [Color.Red] = ConsoleColor.Red,
            [Color.Magenta] = ConsoleColor.Magenta,
            [Color.Yellow] = ConsoleColor.Yellow,
            [Color.White] = ConsoleColor.White
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
