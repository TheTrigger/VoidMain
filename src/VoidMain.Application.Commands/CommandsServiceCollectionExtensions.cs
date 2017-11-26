using VoidMain.Application.Commands;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommandsServiceCollectionExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddTransient<ICommandsApplication, CommandsApplication>();
            return services;
        }
    }
}
