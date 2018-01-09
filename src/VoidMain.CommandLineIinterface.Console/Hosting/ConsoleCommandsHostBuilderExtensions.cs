using Microsoft.Extensions.DependencyInjection;

namespace VoidMain.Hosting
{
    public static class ConsoleCommandsHostBuilderExtensions
    {
        public static ICommandsHostBuilder UseSimpleConsole(this ICommandsHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services
                    .AddConsoleInterface()
                    .AddCmdPrompt();
            });
        }

        public static ICommandsHostBuilder UseAdvancedConsole(this ICommandsHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                var interfaceBuilder = services
                    .AddConsoleInterface()
                    .AddCmdPrompt()
                    .AddUndoRedo()
                    .AddSyntaxHighlighting();

                interfaceBuilder
                    .AddCommandsHistory()
                    .AddFileStorage();
            });
        }
    }
}
