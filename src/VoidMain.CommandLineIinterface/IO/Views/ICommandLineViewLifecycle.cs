namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ICommandLineViewLifecycle
    {
        void BeginReadingLine();
        void EndReadingLine();

        void BeginHandlingInput();
        void EndHandlingInput();
    }
}
