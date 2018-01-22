using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands
{
    public interface ICommandsModule
    {
        ICommandLineOutput Output { get; set; }
    }
}
