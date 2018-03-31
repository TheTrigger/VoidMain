namespace VoidMain.CommandLineIinterface.IO.InputHandlers
{
    public interface IConsoleInputHandler
    {
        int Order { get; }
        void Handle(ConsoleInputEventArgs args);
    }
}
