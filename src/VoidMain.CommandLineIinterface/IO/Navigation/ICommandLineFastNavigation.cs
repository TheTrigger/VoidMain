using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO.Navigation
{
    public interface ICommandLineFastNavigation
    {
        int FindNext(ICommandLineReadOnlyView lineView);
        int FindPrev(ICommandLineReadOnlyView lineView);
    }
}
