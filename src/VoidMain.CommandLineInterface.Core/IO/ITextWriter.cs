namespace VoidMain.CommandLineInterface.IO
{
    public interface ITextWriter
    {
        void Write(char value);
        void Write(char value, int count);
        void Write(string value);
        void Write(object value);
        void WriteLine();
    }
}
