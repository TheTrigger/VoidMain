using VoidMain.Application.Builder;

namespace VoidMain.Hosting
{
    public static class CliApp
    {
        public static void Run<TStartup>()
            where TStartup : class, IStartup
        {
            var host = new CommandsHostBuilder()
                .UseStartup<TStartup>()
                .Build();

            using (host)
            {
                host.Run();
            }
        }

        public static void RunWithCustomDI<TStartup>()
            where TStartup : class, IStartupWithCustomDI
        {
            var host = new CommandsHostBuilder()
                .UseStartupWithCustomDI<TStartup>()
                .Build();

            using (host)
            {
                host.Run();
            }
        }
    }
}
