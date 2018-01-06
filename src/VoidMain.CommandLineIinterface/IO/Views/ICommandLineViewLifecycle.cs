namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineViewLifecycle
    {
        void BeginReadingLine();
        void EndReadingLine();

        void BeginHandlingInput();
        void EndHandlingInput();
    }
}
