namespace VoidMain.CommandLineIinterface.Console
{
    public interface ICommandLineViewManager
    {
        ICommandLineView GetView();
        void StartChanges();
        void EndChanges();
        void ResetState();
    }
}
