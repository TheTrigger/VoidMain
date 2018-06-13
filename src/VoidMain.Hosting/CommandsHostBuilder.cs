using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using VoidMain.Application.Builder;
using VoidMain.CommandLineIinterface;
using VoidMain.Hosting.FileSystem;

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

            IServiceProvider hostServices = null;
            IServiceProvider services = null;
            try
            {
                hostServices = serviceCollection.BuildServiceProvider();

                var startup = hostServices.GetService<IStartupWithCustomDI>();
                if (startup == null)
                {
                    var startupWithDefaultDI = hostServices.GetRequiredService<IStartup>();
                    startup = new StartupWithDefaultDI(startupWithDefaultDI);
                }

                services = startup.ConfigureServices(serviceCollection);
                var appBuilder = services.GetRequiredService<IApplicationBuilder>();
                startup.ConfigureApplication(appBuilder);

                var app = appBuilder.Build();
                var cli = services.GetRequiredService<ICommandLineInterface>();

                return new CommandsHost(services, cli, app);
            }
            catch
            {
                (services as IDisposable)?.Dispose();
                throw;
            }
            finally
            {
                (hostServices as IDisposable)?.Dispose();
            }
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
            services.AddSingleton<IFileSystem, PhisicalFileSystem>();
            services.AddSingleton<IClock, SystemClock>();
        }
    }
}
