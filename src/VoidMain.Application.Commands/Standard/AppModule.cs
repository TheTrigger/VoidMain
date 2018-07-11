using VoidMain.CommandLineInterface;

namespace VoidMain.Application.Commands.Standard
{
    [Module(ExcludeFromCommandName = true)]
    public class AppModule
    {
        [Command(Description = "Close the application")]
        public void Quit(ICommandLineInterface cli)
        {
            // Do not await it because the app will wait
            // until the command execution is over.
            cli.StopAsync();
        }
    }
}
