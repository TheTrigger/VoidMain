using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.Hosting
{
    public static class CommandsHostExtensions
    {
        public static void Run(this ICommandsHost host)
        {
            host.RunAsync().GetAwaiter().GetResult();
        }

        public static async Task RunAsync(this ICommandsHost host, CancellationToken token = default)
        {
            using (host)
            {
                await host.StartAsync(token).ConfigureAwait(false);
                await host.WaitForShutdownAsync(token).ConfigureAwait(false);
            }
        }
    }
}
