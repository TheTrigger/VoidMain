using System;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Builder;
using VoidMain.CommandLineInterface;

namespace VoidMain.Hosting
{
    public class CommandsHost : ICommandsHost
    {
        private readonly IServiceProvider _services;
        private readonly ICommandLineInterface _cli;
        private readonly CommandDelegate _app;
        private bool _isDisposed;

        public CommandsHost(
            IServiceProvider services,
            ICommandLineInterface cli,
            CommandDelegate app)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _cli = cli ?? throw new ArgumentNullException(nameof(cli));
            _app = app ?? throw new ArgumentNullException(nameof(app));
            _isDisposed = false;
        }

        public async Task StartAsync(CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            await _cli.StartAsync(_app, token).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken token = default)
        {
            await _cli.StopAsync(token).ConfigureAwait(false);
            await WaitForShutdownAsync(token).ConfigureAwait(false);
        }

        public Task WaitForShutdownAsync(CancellationToken token = default)
        {
            return _cli.WaitForShutdownAsync(token);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose()
        {
            _isDisposed = true;
            (_services as IDisposable)?.Dispose();
        }
    }
}
