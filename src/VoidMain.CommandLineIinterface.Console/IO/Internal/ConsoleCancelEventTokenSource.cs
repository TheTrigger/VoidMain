using System;
using System.Threading;

namespace VoidMain.CommandLineIinterface.IO.Console.Internal
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
            _console.CancelKeyPress += Canceled;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _console.CancelKeyPress -= Canceled;
            _tokenSource.Dispose();
        }

        private void Canceled(object sender, ConsoleCancelEventArgs e)
        {
            if (_isDisposed) return;

            lock (_tokenSource)
            {
                e.Cancel = true;
                // ISSUE: `CancelKeyPress -= Canceled` hangs if token
                // was canceled inside ConsoleCancelEventHandler.
                // WORKAROUND: Use `CancelAfter(0)` because it starts
                // a timer and performs cancelation in another thread.
                _tokenSource.CancelAfter(0);
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
