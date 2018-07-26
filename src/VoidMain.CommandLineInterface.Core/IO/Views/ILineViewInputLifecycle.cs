namespace VoidMain.CommandLineInterface.IO.Views
{
    public interface ILineViewInputLifecycle
    {
        void BeforeLineReading();
        void AfterLineReading();

        void BeforeInputHandling(bool hasMoreInput);
        void AfterInputHandling(bool hasMoreInput);
    }
}
