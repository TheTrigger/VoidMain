using System;
using VoidMain.CommandLineIinterface.History;
using VoidMain.CommandLineIinterface.IO.Console.InputHandlers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommandsHistoryServiceCollectionExtensions
    {
        public static CommandsHistoryBuilder AddCommandsHistory(
            this IServiceCollection services, Action<CommandsHistoryOptions> options = null)
        {
            services.AddTransient<IConsoleInputHandler, CommandsHistoryInputHandler>();
            services.AddSingleton<ICommandsHistoryManager, CommandsHistoryManager>();
            if (options != null)
            {
                services.Configure(options);
            }
            return new CommandsHistoryBuilder(services);
        }

        public static CommandsHistoryBuilder AddFileStorage(
            this CommandsHistoryBuilder builder, Action<CommandsHistoryFileStorageOptions> options = null)
        {
            builder.Services.AddTransient<ICommandsHistoryStorage, CommandsHistoryFileStorage>();
            if (options != null)
            {
                builder.Services.Configure(options);
            }
            return builder;
        }

        public static CommandsHistoryBuilder AddInMemoryStorage(
            this CommandsHistoryBuilder builder, Action<CommandsHistoryInMemoryStorageOptions> options = null)
        {
            builder.Services.AddTransient<ICommandsHistoryStorage, CommandsHistoryInMemoryStorage>();
            if (options != null)
            {
                builder.Services.Configure(options);
            }
            return builder;
        }
    }
}
