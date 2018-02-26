namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ICommandLineViewNavigation
    {
        int FindNextPosition(ICommandLineReadOnlyView lineView);
        int FindPrevPosition(ICommandLineReadOnlyView lineView);
    }
}
