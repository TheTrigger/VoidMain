namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineFastNavigation
    {
        int FindNext(ICommandLineReadOnlyView lineView);
        int FindPrev(ICommandLineReadOnlyView lineView);
    }
}
