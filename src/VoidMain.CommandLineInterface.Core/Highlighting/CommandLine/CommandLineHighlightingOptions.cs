using System;

namespace VoidMain.CommandLineInterface.Highlighting.CommandLine
{
    public class CommandLineHighlightingOptions
    {
        public HighlightingPalette<CommandLineStyleName, TextStyle> Palette { get; set; }

        public CommandLineHighlightingOptions(bool defaults = true)
        {
            if (defaults)
            {
                Palette = CommandLineHighlightingPalette.Default;
            }
        }

        public void Validate()
        {
            if (Palette == null)
            {
                throw new ArgumentNullException(nameof(Palette));
            }
        }
    }
}
