using System;
using VoidMain.CommandLineIinterface.IO.Console.Internal;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight.Console
{
    public class DefaultConsoleSyntaxHighlightingPallete : SyntaxHighlightingPallete<ConsoleTextStyle>
    {
        public DefaultConsoleSyntaxHighlightingPallete(IConsole console)
            : base(new ConsoleTextStyle(console.BackgroundColor, console.ForegroundColor))
        {
            Add(SyntaxClass.CommandName, new ConsoleTextStyle(ConsoleColor.Yellow));
            Add(SyntaxClass.OptionNameMarker, new ConsoleTextStyle(ConsoleColor.DarkGray));
            Add(SyntaxClass.OptionName, new ConsoleTextStyle(ConsoleColor.DarkGray));
            Add(SyntaxClass.OptionValueMarker, new ConsoleTextStyle(ConsoleColor.DarkGray));
            Add(SyntaxClass.OptionValue, new ConsoleTextStyle(ConsoleColor.White));
            Add(SyntaxClass.OperandsSectionMarker, new ConsoleTextStyle(ConsoleColor.DarkMagenta));
            Add(SyntaxClass.Operand, new ConsoleTextStyle(ConsoleColor.DarkCyan));
        }
    }
}
