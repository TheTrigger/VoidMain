namespace VoidMain.CommandLineInterface.IO
{
    public interface IColoredTextWriter : ITextWriter
    {
        void Write(Color foreground, Color background, char value);
        void Write(Color foreground, Color background, char value, int count);
        void Write(Color foreground, Color background, string value);
        void Write(Color foreground, Color background, object value);
    }
}
