using System;
using VoidMain.CommandLineIinterface.IO.Console.Internal;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight.Console
{
    public class DefaultConsoleSyntaxHighlightingPallete : SyntaxHighlightingPallete<ConsoleTextStyle>
    {
        private static readonly ConsoleColor? CurrentColor = null;

        public DefaultConsoleSyntaxHighlightingPallete(IConsole console)
            : base(new ConsoleTextStyle(console.BackgroundColor, console.ForegroundColor))
        {
            Add(SyntaxClass.CommandName, new ConsoleTextStyle(CurrentColor, ConsoleColor.Yellow));
            Add(SyntaxClass.OptionNameMarker, new ConsoleTextStyle(CurrentColor, ConsoleColor.DarkGray));
            Add(SyntaxClass.OptionName, new ConsoleTextStyle(CurrentColor, ConsoleColor.DarkGray));
            Add(SyntaxClass.OptionValueMarker, new ConsoleTextStyle(CurrentColor, ConsoleColor.DarkGray));
            Add(SyntaxClass.OptionValue, new ConsoleTextStyle(CurrentColor, ConsoleColor.White));
            Add(SyntaxClass.OperandsSectionMarker, new ConsoleTextStyle(CurrentColor, ConsoleColor.DarkMagenta));
            Add(SyntaxClass.Operand, new ConsoleTextStyle(CurrentColor, ConsoleColor.DarkCyan));
        }
    }
}
