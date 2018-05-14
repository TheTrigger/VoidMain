namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ILineViewNavigation
    {
        int FindNextPosition(IReadOnlyLineView lineView);
        int FindPrevPosition(IReadOnlyLineView lineView);
    }
}
