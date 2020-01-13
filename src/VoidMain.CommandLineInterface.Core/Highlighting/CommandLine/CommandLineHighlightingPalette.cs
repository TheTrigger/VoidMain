namespace VoidMain.CommandLineInterface.Highlighting.CommandLine
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
                { CommandLineStyleName.CommandName, Colors.Yellow },
                { CommandLineStyleName.OptionNameMarker, Colors.DarkGray },
                { CommandLineStyleName.OptionName, Colors.DarkGray },
                { CommandLineStyleName.OptionValueMarker, Colors.DarkGray },
                { CommandLineStyleName.OptionValue, Colors.White },
                { CommandLineStyleName.EndOfOptions, Colors.DarkMagenta },
                { CommandLineStyleName.Operand, Colors.DarkCyan }
            };
    }
}
