using System;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.CommandLineIinterface.Console
{
    public class DefaultConsoleSyntaxPallete : SyntaxPallete<ConsoleColor?>
    {
        private static readonly ConsoleColor? CurrentColor = null;

        public DefaultConsoleSyntaxPallete(IConsole console)
            : base(console.BackgroundColor, console.ForegroundColor)
        {
            Add(SyntaxClass.CommandName, CurrentColor, ConsoleColor.Yellow);
            Add(SyntaxClass.OptionNameMarker, CurrentColor, ConsoleColor.DarkGray);
            Add(SyntaxClass.OptionName, CurrentColor, ConsoleColor.DarkGray);
            Add(SyntaxClass.OptionValueMarker, CurrentColor, ConsoleColor.DarkGray);
            Add(SyntaxClass.OptionValue, CurrentColor, ConsoleColor.White);
            Add(SyntaxClass.OperandsSectionMarker, CurrentColor, ConsoleColor.DarkMagenta);
            Add(SyntaxClass.Operand, CurrentColor, ConsoleColor.DarkCyan);
        }
    }
}
