namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineOutput
    {
        void WriteLine();
        void WriteLine(string value);
        void WriteLine(object value);
        void WriteLine(string format, params object[] args);
        void Clear();
    }
}
