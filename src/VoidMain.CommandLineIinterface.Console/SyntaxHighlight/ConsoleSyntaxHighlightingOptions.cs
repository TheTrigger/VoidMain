using System;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class ConsoleSyntaxHighlightingOptions
    {
        public SyntaxHighlightingPallete<ConsoleTextStyle> Pallete { get; set; }

        public ConsoleSyntaxHighlightingOptions(bool defaults = true)
        {
            if (defaults)
            {
                Pallete = ConsoleSyntaxHighlightingPallete.Default;
            }
        }

        public void Validate()
        {
            if (Pallete == null)
            {
                throw new ArgumentNullException(nameof(Pallete));
            }
        }
    }
}
