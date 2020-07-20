using System;
using System.Threading;
using VoidMain.IO.Console;

namespace Experiments.Design.IO.TextEditors.Console
{
    public class ConsoleCancelKeyTokenSource : IDisposable
    {
        private static readonly WaitCallback CancelCallback = CancelTokens;

        private readonly IConsole _console;
        private bool _isDisposed;
        private CancellationTokenSource _tokenSource;

        public CancellationToken Token => _tokenSource.Token;
        public bool IsCancellationRequested => _tokenSource.IsCancellationRequested;

        public ConsoleCancelKeyTokenSource(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _isDisposed = false;
            _tokenSource = new CancellationTokenSource();
            _console.CancelKeyPress += OnCancelKeyPressed;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _console.CancelKeyPress -= OnCancelKeyPressed;
            lock (_tokenSource) _tokenSource.Dispose();
            _isDisposed = true;
        }

        private void OnCancelKeyPressed(object sender, ConsoleCancelEventArgs e)
        {
            if (_isDisposed) return;

            lock (_tokenSource)
            {
                e.Cancel = true;
                ThreadPool.QueueUserWorkItem(CancelCallback, _tokenSource);
            }
        }

        private static void CancelTokens(object? state)
        {
            ((CancellationTokenSource)state!).Cancel();
        }

        public void ManualCancel()
        {
            ThrowIfDisposed();
            lock (_tokenSource)
            {
                _tokenSource.Cancel();
            }
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
