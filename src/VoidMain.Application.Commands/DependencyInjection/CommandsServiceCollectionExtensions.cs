using VoidMain.Application.Commands.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommandsServiceCollectionExtensions
    {
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddTransient<ICommandsApplicationBuilder, CommandsApplicationBuilder>();
            services.AddTransient<IModuleModelConstructor, ModuleModelConstructor>();
            services.AddTransient<ICommandModelConstructor, CommandModelConstructor>();
            services.AddTransient<IArgumentModelConstructor, ArgumentModelConstructor>();
            services.AddTransient<IModuleConfigurationFactory, ModuleConfigurationFactory>();
            return services;
        }
    }
}
