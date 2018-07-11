namespace VoidMain.CommandLineIinterface.Highlighting.CommandLine
{
    public class CommandLineHighlightingPalette : HighlightingPalette<CommandLineStyleName, TextStyle>
    {
        public CommandLineHighlightingPalette(
            TextStyle defaultStyle = null)
            : base(defaultStyle)
        {
        }

        public void Add(CommandLineStyleName name, Color foreground)
        {
            Add(name, new TextStyle(foreground));
        }

        public void Add(CommandLineStyleName name, Color foreground, Color background)
        {
            Add(name, new TextStyle(foreground, background));
        }

        public static CommandLineHighlightingPalette Default =>
            new CommandLineHighlightingPalette()
            {
                { CommandLineStyleName.CommandName, Color.Yellow },
                { CommandLineStyleName.OptionNameMarker, Color.DarkGray },
                { CommandLineStyleName.OptionName, Color.DarkGray },
                { CommandLineStyleName.OptionValueMarker, Color.DarkGray },
                { CommandLineStyleName.OptionValue, Color.White },
                { CommandLineStyleName.EndOfOptions, Color.DarkMagenta },
                { CommandLineStyleName.Operand, Color.DarkCyan }
            };
    }
}
