namespace VoidMain.CommandLineIinterface.Console
{
    public interface ICommandLineFastNavigation
    {
        int FindNext(ICommandLineView lineView);
        int FindPrev(ICommandLineView lineView);
    }
}
