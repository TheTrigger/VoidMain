using System;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class SyntaxHighlightingOptions
    {
        public SyntaxHighlightingPalette<TextStyle> Palette { get; set; }

        public SyntaxHighlightingOptions(bool defaults = true)
        {
            if (defaults)
            {
                Palette = SyntaxHighlightingPalette.Default;
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
