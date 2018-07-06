namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class SyntaxHighlightingPalette : SyntaxHighlightingPalette<TextStyle>
    {
        public static SyntaxHighlightingPalette Default => new DefaultPalette();

        public SyntaxHighlightingPalette(
            TextStyle defaultStyle = null)
            : base(defaultStyle)
        {
        }

        public void Add(SyntaxClass @class, Color foreground)
        {
            Add(@class, new TextStyle(foreground));
        }

        public void Add(SyntaxClass @class, Color foreground, Color background)
        {
            Add(@class, new TextStyle(foreground, background));
        }

        private class DefaultPalette : SyntaxHighlightingPalette
        {
            public DefaultPalette()
                : base(TextStyle.Default)
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
