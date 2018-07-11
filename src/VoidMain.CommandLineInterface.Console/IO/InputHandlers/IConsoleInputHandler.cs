namespace VoidMain.CommandLineInterface.IO.InputHandlers
{
    public interface IConsoleInputHandler
    {
        int Order { get; }
        void Handle(ConsoleInputEventArgs args);
    }
}
