using System;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight.Console
{
    public class ConsoleTextStyle
    {
        public ConsoleColor? Background { get; }
        public ConsoleColor? Foreground { get; }

        public ConsoleTextStyle(ConsoleColor? foreground)
        {
            Background = null;
            Foreground = foreground;
        }

        public ConsoleTextStyle(
            ConsoleColor? background, ConsoleColor? foreground)
        {
            Background = background;
            Foreground = foreground;
        }

        public override string ToString()
        {
            return $"{{{Background}, {Foreground}}}";
        }
    }
}
