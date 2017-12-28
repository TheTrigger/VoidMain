namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineFastNavigation
    {
        int FindNext(ICommandLineView lineView);
        int FindPrev(ICommandLineView lineView);
    }
}
