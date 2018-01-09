namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ICommandLineViewSelector
    {
        ICommandLineView SelectView(CommandLineViewOptions options);
    }
}
