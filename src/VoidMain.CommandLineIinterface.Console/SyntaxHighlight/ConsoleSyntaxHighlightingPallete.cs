using System;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class ConsoleSyntaxHighlightingPallete : SyntaxHighlightingPallete<ConsoleTextStyle>
    {
        public static ConsoleSyntaxHighlightingPallete Default => new DefaultPallete();

        public ConsoleSyntaxHighlightingPallete(
            ConsoleTextStyle defaultStyle = null)
            : base(defaultStyle)
        {
        }

        public void Add(SyntaxClass @class, ConsoleColor foreground)
        {
            Add(@class, new ConsoleTextStyle(foreground));
        }

        public void Add(SyntaxClass @class, ConsoleColor foreground, ConsoleColor background)
        {
            Add(@class, new ConsoleTextStyle(foreground, background));
        }

        private class DefaultPallete : ConsoleSyntaxHighlightingPallete
        {
            public DefaultPallete()
                : base(ConsoleTextStyle.Default)
            {
                Add(SyntaxClass.CommandName, ConsoleColor.Yellow);
                Add(SyntaxClass.OptionNameMarker, ConsoleColor.DarkGray);
                Add(SyntaxClass.OptionName, ConsoleColor.DarkGray);
                Add(SyntaxClass.OptionValueMarker, ConsoleColor.DarkGray);
                Add(SyntaxClass.OptionValue, ConsoleColor.White);
                Add(SyntaxClass.OperandsSectionMarker, ConsoleColor.DarkMagenta);
                Add(SyntaxClass.Operand, ConsoleColor.DarkCyan);
            }
        }
    }
}
