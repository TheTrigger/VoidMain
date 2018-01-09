namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ICommandLineViewProvider
    {
        CommandLineViewType ViewType { get; }
        ICommandLineView GetView(CommandLineViewOptions options);
    }
}
