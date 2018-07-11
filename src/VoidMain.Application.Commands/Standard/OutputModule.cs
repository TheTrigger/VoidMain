using VoidMain.CommandLineInterface;

namespace VoidMain.Application.Commands.Standard
{
    [Module(ExcludeFromCommandName = true)]
    public class OutputModule
    {
        [Command(Description = "Clear the screen")]
        public void Clear(ICommandLineOutput output)
        {
            output.Clear();
        }
    }
}
