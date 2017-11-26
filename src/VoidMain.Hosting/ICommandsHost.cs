using System;
using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.Hosting
{
    public interface ICommandsHost : IDisposable
    {
        Task StartAsync(CancellationToken token = default(CancellationToken));
        Task StopAsync(CancellationToken token = default(CancellationToken));
        Task WaitForShutdownAsync(CancellationToken token = default(CancellationToken));
    }
}
