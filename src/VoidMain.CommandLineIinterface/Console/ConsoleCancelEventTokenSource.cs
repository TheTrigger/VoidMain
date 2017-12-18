using System;
using System.Threading;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleCancelEventTokenSource : IDisposable
    {
        private readonly IConsole _console;
        private bool _isDisposed;
        private CancellationTokenSource _tokenSource;

        public CancellationToken Token => _tokenSource.Token;
        public bool IsCancellationRequested => _tokenSource.IsCancellationRequested;

        public ConsoleCancelEventTokenSource(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _isDisposed = false;
            _tokenSource = new CancellationTokenSource();
            _console.CancelKeyPress += Cancelled;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _console.CancelKeyPress -= Cancelled;
            _tokenSource.Dispose();
        }

        private void Cancelled(object sender, ConsoleCancelEventArgs e)
        {
            if (e.SpecialKey != ConsoleSpecialKey.ControlC || _isDisposed)
            {
                return;
            }
            lock (_tokenSource)
            {
                e.Cancel = true;
                _tokenSource.Cancel();
            }
        }

        public void Cancel()
        {
            ThrowIfDisposed();
            _tokenSource.Cancel();
        }

        public void Reset()
        {
            ThrowIfDisposed();
            lock (_tokenSource)
            {
                if (_tokenSource.IsCancellationRequested)
                {
                    _tokenSource.Dispose();
                    _tokenSource = new CancellationTokenSource();
                }
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
    }
}
