using System;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class ConsoleSyntaxHighlightingOptions
    {
        public SyntaxHighlightingPalette<ConsoleTextStyle> Palette { get; set; }

        public ConsoleSyntaxHighlightingOptions(bool defaults = true)
        {
            if (defaults)
            {
                Palette = ConsoleSyntaxHighlightingPalette.Default;
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
