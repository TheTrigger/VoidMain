namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class CommandLineHiddenViewProvider : ICommandLineViewProvider
    {
        public CommandLineViewType ViewType { get; } = CommandLineViewType.Hidden;

        public ICommandLineView GetView(CommandLineViewOptions options)
        {
            return new CommandLineHiddenView();
        }
    }
}
