using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace VoidMain.IO.Console.Internal
{
    public class AsyncPolling<TState> : IValueTaskSource, IDisposable
    {
        private static readonly TimerCallback OnPollCallback = OnPoll;
        private static readonly Action<object?> OnCancelCallback = OnCancel;

        // Mutable struct. Do not make this readonly.
        private ManualResetValueTaskSourceCore<bool> _taskSource;
        private readonly Timer _timer;
        private readonly int _period;
        private Func<TState, bool> _isCompleted;
        private TState _state;
        private CancellationToken _token;
        private int _gate;
        private bool _isDisposed;

        public AsyncPolling(TimeSpan period, TState state, Func<TState, bool> isCompleted)
            : this((int)period.TotalMilliseconds, state, isCompleted) { }

        public AsyncPolling(int period, TState state, Func<TState, bool> isCompleted)
        {
            if (period < 1 || period > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }
            _period = period;
            _isCompleted = isCompleted ?? throw new ArgumentNullException(nameof(isCompleted));
            _state = state;
            _timer = new Timer(OnPollCallback, this, Timeout.Infinite, Timeout.Infinite);
            _taskSource.RunContinuationsAsynchronously = true;
            _isDisposed = false;
        }

        ~AsyncPolling() => Dispose();

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref _gate, 1, 0) == 0)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                _taskSource.SetException(new ObjectDisposedException(null));
            }

            _timer.Dispose();
            _isDisposed = true;
        }

        public ValueTask PollAsync(CancellationToken token = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(null);
            }

            if (_isCompleted(_state))
            {
                return new ValueTask();
            }

            _taskSource.Reset();

            if (token.IsCancellationRequested)
            {
                _taskSource.SetException(new OperationCanceledException(token));
                return new ValueTask(this, _taskSource.Version);
            }

            if (token.CanBeCanceled)
            {
                _token = token;
                token.Register(OnCancelCallback, this);
            }

            _gate = 0;
            _timer.Change(_period, _period);

            return new ValueTask(this, _taskSource.Version);
        }

        private static void OnPoll(object? state)
        {
            var target = (AsyncPolling<TState>)state!;

            if (!target._isCompleted(target._state))
            {
                return;
            }

            if (Interlocked.CompareExchange(ref target._gate, 1, 0) == 0)
            {
                target._timer.Change(Timeout.Infinite, Timeout.Infinite);
                target._taskSource.SetResult(true);
            }
        }

        private static void OnCancel(object? state)
        {
            var target = (AsyncPolling<TState>)state!;

            if (Interlocked.CompareExchange(ref target._gate, 1, 0) == 0)
            {
                target._timer.Change(Timeout.Infinite, Timeout.Infinite);
                target._taskSource.SetException(new OperationCanceledException(target._token));
            }
        }

        void IValueTaskSource.GetResult(short token)
            => _taskSource.GetResult(token);

        ValueTaskSourceStatus IValueTaskSource.GetStatus(short token)
            => _taskSource.GetStatus(token);

        void IValueTaskSource.OnCompleted(Action<object?> continuation,
            object? state, short token, ValueTaskSourceOnCompletedFlags flags)
            => _taskSource.OnCompleted(continuation, state, token, flags);
    }
}
