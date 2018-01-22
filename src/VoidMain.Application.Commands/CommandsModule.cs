using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands
{
    public abstract class CommandsModule : ICommandsModule
    {
        public ICommandLineOutput Output { get; set; }
    }
}
