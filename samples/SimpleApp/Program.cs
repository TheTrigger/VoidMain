using Microsoft.Extensions.DependencyInjection;
using VoidMain.Application.Builder;
using VoidMain.Hosting;

namespace SimpleApp
{
    class Program : IStartup
    {
        static void Main(string[] args)
        {
            var host = new CommandsHostBuilder()
                .UseSimpleConsole()
                .UseStartup<Program>()
                .Build();

            host.Run();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCommands();
        }

        public void ConfigureApplication(IApplicationBuilder app)
        {
            app.RunCommands();
        }
    }
}
