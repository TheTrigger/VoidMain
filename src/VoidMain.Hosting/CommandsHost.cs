using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Builder;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Hosting
{
    public class CommandsHost : ICommandsHost
    {
        private IServiceCollection _serviceCollection;
        private IServiceProvider _appServices;
        private ICommandLineIinterface _cli;
        private CommandDelegate _app;
        private bool _isDisposed;

        public CommandsHost(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _isDisposed = false;
        }

        public Task StartAsync(CancellationToken token = default(CancellationToken))
        {
            ThrowIfDisposed();
            token.ThrowIfCancellationRequested();
            EnsureApplicationBuilt();
            return _cli.StartAsync(_app, token);
        }

        public async Task StopAsync(CancellationToken token = default(CancellationToken))
        {
            await _cli.StopAsync(token).ConfigureAwait(false);
            await WaitForShutdownAsync(token).ConfigureAwait(false);
        }

        public Task WaitForShutdownAsync(CancellationToken token = default(CancellationToken))
        {
            return _cli.WaitForShutdownAsync(token);
        }

        public void EnsureApplicationBuilt()
        {
            if (_app != null && _cli != null)
            {
                return;
            }

            try
            {
                var hostServices = _serviceCollection.BuildServiceProvider();

                var startup = hostServices.GetService<IStartupWithCustomDI>();
                if (startup == null)
                {
                    var startupWithDefaultDI = hostServices.GetRequiredService<IStartup>();
                    startup = new StartupWithDefaultDI(startupWithDefaultDI);
                }

                (hostServices as IDisposable)?.Dispose();

                _appServices = startup.ConfigureServices(_serviceCollection);
                var appBuilder = _appServices.GetRequiredService<IApplicationBuilder>();
                startup.ConfigureApplication(appBuilder);

                _app = appBuilder.Build();
                _cli = _appServices.GetRequiredService<ICommandLineIinterface>();

                // We don't need this anymore because the host can be initialized only once.
                (_serviceCollection as IDisposable)?.Dispose();
                _serviceCollection = null;
            }
            catch
            {
                (_appServices as IDisposable)?.Dispose();
                _appServices = null;
                _app = null;
                _cli = null;
                throw;
            }
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
            (_serviceCollection as IDisposable)?.Dispose();
            (_appServices as IDisposable)?.Dispose();
        }
    }
}
