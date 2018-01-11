using Microsoft.Extensions.DependencyInjection;
using System;
using VoidMain.Application.Builder;
using VoidMain.Application.Commands.Builder;
using VoidMain.Hosting;

namespace SimpleApp
{
    class Program : IStartup
    {
        static void Main(string[] args)
        {
            PrintDevelopmentNote();

            var host = new CommandsHostBuilder()
                .UseAdvancedConsole()
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
            app.RunCommands(commands =>
            {
                commands.AddModule<GreetingsModule>();
            });
        }

        private static void PrintDevelopmentNote()
        {
            Console.WriteLine("=======================================================");
            Console.WriteLine("This framework is still in early development.");
            Console.WriteLine("See README.md to learn what features are available.");
            Console.WriteLine("Type 'quit' or press Ctrl+C twice to close application.");
            Console.WriteLine("=======================================================");
            Console.WriteLine();
        }
    }

    public class GreetingsModule
    {
        public void Hello(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }
    }
}
