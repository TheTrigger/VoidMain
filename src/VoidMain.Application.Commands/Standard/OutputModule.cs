using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Standard
{
    [Module(ExcludeFromCommandName = true)]
    public class OutputModule
    {
        public void Clear(ICommandLineOutput output)
        {
            output.Clear();
        }
    }
}
