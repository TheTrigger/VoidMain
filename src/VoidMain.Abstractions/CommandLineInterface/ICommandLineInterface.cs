using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Builder;

namespace VoidMain.CommandLineInterface
{
    public interface ICommandLineInterface
    {
        Task StartAsync(CommandDelegate application, CancellationToken token = default);
        Task StopAsync(CancellationToken token = default);
        Task WaitForShutdownAsync(CancellationToken token = default);
    }
}
