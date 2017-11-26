using Microsoft.Extensions.DependencyInjection;
using VoidMain.CommandLineIinterface;
using VoidMain.CommandLineIinterface.Console;
using VoidMain.CommandLineIinterface.Parser;

namespace VoidMain.Hosting
{
    public static class ConsoleCommandsHostBuilderExtensions
    {
        public static ICommandsHostBuilder UseSimpleConsole(this ICommandsHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IConsole>(_ => PlatformConsole.Instance);
                services.AddSingleton<ICommandLineIinterface, SimpleConsoleCommandLineIinterface>();
                services.AddTransient<ICommandLineOutput, CommandLineOutput>();
                services.AddTransient<ICommandLineParser, CommandLineParser>();
            });
        }
    }
}
