namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ICommandLineViewLifecycle
    {
        void BeginReadingLine();
        void EndReadingLine();

        void BeginHandlingInput(bool isNextKeyAvailable);
        void EndHandlingInput(bool isNextKeyAvailable);
    }
}
