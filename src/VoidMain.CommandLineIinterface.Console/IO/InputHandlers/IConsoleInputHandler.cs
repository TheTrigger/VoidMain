namespace VoidMain.CommandLineIinterface.IO.Console.InputHandlers
{
    public interface IConsoleInputHandler
    {
        int Order { get; }
        void Handle(ConsoleInputEventArgs args);
    }
}
