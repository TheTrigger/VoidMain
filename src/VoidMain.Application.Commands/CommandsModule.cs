using VoidMain.CommandLineInterface;

namespace VoidMain.Application.Commands
{
    public abstract class CommandsModule : ICommandsModule
    {
        public ICommandLineOutput Output { get; set; }
    }
}
