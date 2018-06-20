namespace VoidMain.CommandLineIinterface.IO
{
    public interface IColoredTextWriter
    {
        void Write(Color foreground, Color background, string value);
        void Write(Color foreground, Color background, object value);
        void Write(Color foreground, Color background, char value);
        void Write(Color foreground, Color background, char value, int length);
        void WriteLine();
    }
}
