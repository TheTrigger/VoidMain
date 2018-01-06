namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineViewProvider
    {
        ICommandLineView GetView(CommandLineViewOptions options);
    }
}
