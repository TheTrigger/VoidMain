namespace VoidMain.CommandLineInterface.IO.InputHandlers
{
    public interface IInputHandler
    {
        int Order { get; }
        void Handle(InputEventArgs args);
    }
}
