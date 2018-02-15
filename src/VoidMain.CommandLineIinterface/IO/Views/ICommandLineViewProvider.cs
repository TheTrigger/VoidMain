namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ICommandLineViewProvider
    {
        ICommandLineView GetView(CommandLineViewOptions options);
    }
}
