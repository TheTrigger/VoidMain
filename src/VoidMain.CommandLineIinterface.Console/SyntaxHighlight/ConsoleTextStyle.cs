using System;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class ConsoleTextStyle
    {
        public static ConsoleTextStyle Default { get; } = new ConsoleTextStyle(null);

        public ConsoleColor? Foreground { get; }
        public ConsoleColor? Background { get; }

        public ConsoleTextStyle(ConsoleColor? foreground,
            ConsoleColor? background = null)
        {
            Foreground = foreground;
            Background = background;
        }

        public override string ToString()
        {
            return Background == null
                ? $"{{{Foreground}}}"
                : $"{{{Foreground}, {Background}}}";
        }
    }
}
