﻿using System;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleTextStyle
    {
        public ConsoleColor? Background { get; }
        public ConsoleColor? Foreground { get; }

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
