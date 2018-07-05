namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class ConsoleSyntaxHighlightingPalette : SyntaxHighlightingPalette<ConsoleTextStyle>
    {
        public static ConsoleSyntaxHighlightingPalette Default => new DefaultPalette();

        public ConsoleSyntaxHighlightingPalette(
            ConsoleTextStyle defaultStyle = null)
            : base(defaultStyle)
        {
        }

        public void Add(SyntaxClass @class, Color foreground)
        {
            Add(@class, new ConsoleTextStyle(foreground));
        }

        public void Add(SyntaxClass @class, Color foreground, Color background)
        {
            Add(@class, new ConsoleTextStyle(foreground, background));
        }

        private class DefaultPalette : ConsoleSyntaxHighlightingPalette
        {
            public DefaultPalette()
                : base(ConsoleTextStyle.Default)
            {
                Add(SyntaxClass.CommandName, Color.Yellow);
                Add(SyntaxClass.OptionNameMarker, Color.DarkGray);
                Add(SyntaxClass.OptionName, Color.DarkGray);
                Add(SyntaxClass.OptionValueMarker, Color.DarkGray);
                Add(SyntaxClass.OptionValue, Color.White);
                Add(SyntaxClass.EndOfOptions, Color.DarkMagenta);
                Add(SyntaxClass.Operand, Color.DarkCyan);
            }
        }
    }
}
