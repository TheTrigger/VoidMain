using VoidMain.CommandLineInterface;

namespace VoidMain.Application.Commands
{
    public interface ICommandsModule
    {
        ICommandLineOutput Output { get; set; }
    }
}
