using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Builder;

namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineIinterface
    {
        Task StartAsync(CommandDelegate application, CancellationToken token = default(CancellationToken));
        Task StopAsync(CancellationToken token = default(CancellationToken));
        Task WaitForShutdownAsync(CancellationToken token = default(CancellationToken));
    }
}
