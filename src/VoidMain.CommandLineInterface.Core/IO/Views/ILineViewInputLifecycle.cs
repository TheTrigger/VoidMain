namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ILineViewInputLifecycle
    {
        void BeforeLineReading();
        void AfterLineReading();

        void BeforeInputHandling(bool isNextKeyAvailable);
        void AfterInputHandling(bool isNextKeyAvailable);
    }
}
