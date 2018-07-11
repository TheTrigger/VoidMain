namespace VoidMain.CommandLineInterface.IO
{
    public interface IColoredTextWriter
    {
        void Write(Color foreground, Color background, char value);
        void Write(Color foreground, Color background, char value, int count);
        void Write(Color foreground, Color background, string value);
        void Write(Color foreground, Color background, object value);
        void WriteLine();
    }
}
