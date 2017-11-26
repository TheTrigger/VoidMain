using Microsoft.Extensions.DependencyInjection;
using VoidMain.Application.Builder;

namespace VoidMain.Hosting
{
    public static class CommandsHostBuilderExtensions
    {
        public static ICommandsHostBuilder UseStartup<TStartup>(this ICommandsHostBuilder hostBuilder)
            where TStartup : class, IStartup
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<IStartup, TStartup>();
            });
        }

        public static ICommandsHostBuilder UseStartupWithCustomDI<TStartup>(this ICommandsHostBuilder hostBuilder)
            where TStartup : class, IStartupWithCustomDI
        {
            return hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient<IStartupWithCustomDI, TStartup>();
            });
        }
    }
}
