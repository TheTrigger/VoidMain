namespace VoidMain.CommandLineIinterface.Console
{
    public interface ICommandLineReadOnlyView
    {
        int Position { get; }
        int Length { get; }
        char this[int index] { get; }

        string ToString(int start, int length);
        string ToString(int start);
        string ToString();
    }
}
