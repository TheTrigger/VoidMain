namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineOutput
    {
        void Write(char value);
        void Write(string value);
        void Write(object value);
        void Write(string format, params object[] args);

        void WriteLine();
        void WriteLine(string value);
        void WriteLine(object value);
        void WriteLine(string format, params object[] args);

        void Clear();
    }
}
