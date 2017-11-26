using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using VoidMain.Application.Builder;

namespace VoidMain.Hosting
{
    public class CommandsHostBuilder : ICommandsHostBuilder
    {
        private readonly List<Action<IServiceCollection>> _configureServicesDelegates;

        public CommandsHostBuilder(bool useDefaultServices = true)
        {
            _configureServicesDelegates = new List<Action<IServiceCollection>>();
            if (useDefaultServices)
            {
                ConfigureServices(ConfigureDefaultServices);
            }
        }

        public ICommandsHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            if (configureServices == null)
            {
                throw new ArgumentNullException(nameof(configureServices));
            }

            _configureServicesDelegates.Add(configureServices);
            return this;
        }

        public ICommandsHost Build()
        {
            var serviceCollection = ConfigureAllServices();
            var host = new CommandsHost(serviceCollection);
            return host;
        }

        private IServiceCollection ConfigureAllServices()
        {
            var services = new ServiceCollection();
            foreach (var configureServices in _configureServicesDelegates)
            {
                configureServices(services);
            }
            return services;
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            // Default services can be overridden in the following ConfigureServices delegates.
            services.AddTransient<IApplicationBuilder, ApplicationBuilder>();
        }
    }
}
