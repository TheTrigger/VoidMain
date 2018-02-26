using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO.Navigation
{
    public interface ICommandLineViewNavigation
    {
        int FindNextPosition(ICommandLineReadOnlyView lineView);
        int FindPrevPosition(ICommandLineReadOnlyView lineView);
    }
}
