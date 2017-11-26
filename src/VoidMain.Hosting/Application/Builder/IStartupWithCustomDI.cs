using Microsoft.Extensions.DependencyInjection;
using System;

namespace VoidMain.Application.Builder
{
    public interface IStartupWithCustomDI
    {
        IServiceProvider ConfigureServices(IServiceCollection services);
        void ConfigureApplication(IApplicationBuilder app);
    }
}
