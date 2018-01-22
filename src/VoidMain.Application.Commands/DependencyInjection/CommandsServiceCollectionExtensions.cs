using VoidMain.Application.Commands.Arguments;
using VoidMain.Application.Commands.Builder;
using VoidMain.Application.Commands.Execution;
using VoidMain.Application.Commands.Model;
using VoidMain.Application.Commands.Resolving;

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
            services.AddSingleton<ApplicationModel>();
            services.AddTransient<ICommandResolver, CommandResolver>();
            services.AddTransient<IArgumentsParser, ArgumentsParser>();
            services.AddTransient<ICommandExecutor, CommandExecutor>();
            services.AddTransient<ITypeActivator, CachedTypeActivator>();
            services.AddTransient<IMethodInvokerProvider, MethodInvokerProvider>();
            return services;
        }
    }
}
