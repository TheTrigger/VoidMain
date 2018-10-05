using System;
using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.Hosting
{
    public interface ICommandsHost : IDisposable
    {
        Task StartAsync(CancellationToken token = default);
        Task StopAsync(CancellationToken token = default);
        Task WaitForShutdownAsync(CancellationToken token = default);
    }
}
