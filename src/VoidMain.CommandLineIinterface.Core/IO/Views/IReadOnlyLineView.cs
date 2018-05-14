namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface IReadOnlyLineView
    {
        int Position { get; }
        int Length { get; }
        char this[int index] { get; }

        string ToString(int start, int length);
        string ToString(int start);
        string ToString();
    }
}
