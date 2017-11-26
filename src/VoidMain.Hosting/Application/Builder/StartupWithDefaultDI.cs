using Microsoft.Extensions.DependencyInjection;
using System;

namespace VoidMain.Application.Builder
{
    class StartupWithDefaultDI : IStartupWithCustomDI
    {
        private readonly IStartup _startup;

        public StartupWithDefaultDI(IStartup startup)
        {
            _startup = startup ?? throw new ArgumentNullException(nameof(startup));
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _startup.ConfigureServices(services);
            return services.BuildServiceProvider();
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            _startup.ConfigureApplication(app);
        }
    }
}
