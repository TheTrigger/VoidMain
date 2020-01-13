namespace VoidMain.CommandLineInterface.IO
{
    public static class ColoredTextWriterExtensions
    {
        public static void Write(this IColoredTextWriter textWriter, Color? foreground, Color? background, char value)
        {
            textWriter.SetColors(foreground, background);
            textWriter.Write(value);
            textWriter.ResetColors();
        }

        public static void Write(this IColoredTextWriter textWriter, Color? foreground, Color? background, char value, int count)
        {
            if (count <= 0) return;
            textWriter.SetColors(foreground, background);
            textWriter.Write(value, count);
            textWriter.ResetColors();
        }

        public static void Write(this IColoredTextWriter textWriter, Color? foreground, Color? background, string value)
        {
            textWriter.SetColors(foreground, background);
            textWriter.Write(value);
            textWriter.ResetColors();
        }

        public static void Write(this IColoredTextWriter textWriter, Color? foreground, Color? background, object value)
        {
            textWriter.SetColors(foreground, background);
            textWriter.Write(value);
            textWriter.ResetColors();
        }
    }
}
