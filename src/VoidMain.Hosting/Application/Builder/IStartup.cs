using Microsoft.Extensions.DependencyInjection;

namespace VoidMain.Application.Builder
{
    public interface IStartup
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureApplication(IApplicationBuilder app);
    }
}
