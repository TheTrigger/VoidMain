using VoidMain.Application.Builder;

namespace VoidMain.Hosting
{
    public static class CommandsApp
    {
        public static void Start<TStartup>()
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
    }
}
