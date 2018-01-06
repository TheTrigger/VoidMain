namespace VoidMain.CommandLineIinterface.Console
{
    public interface IConsoleInputHandler
    {
        int Order { get; }
        void Handle(ConsoleInputEventArgs args);
    }
}
