namespace VoidMain.CommandLineInterface.IO
{
    public interface IColoredTextWriter : ITextWriter
    {
        void SetColors(Color? foreground, Color? background);
        void ResetColors();
    }
}
