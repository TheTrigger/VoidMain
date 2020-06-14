using System;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.IO.Console.Internal;
using VoidMain.IO.Keyboard;

namespace VoidMain.IO.Console
{
    public class ConsoleKeyReader : IKeyReader, IDisposable
    {
        private readonly IConsole _console;
        private readonly ConsoleKeyConverter _keyConverter;
        private readonly AsyncPolling<IConsole> _polling;
        private readonly bool _intercept;

        public ConsoleKeyReader(IConsole console, ConsoleKeyReaderOptions? options = null)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _intercept = options?.Intercept ?? true;
            var period = options?.PollingPeriod ?? TimeSpan.FromMilliseconds(50.0);
            _keyConverter = new ConsoleKeyConverter();
            _polling = new AsyncPolling<IConsole>(period, _console, console => console.IsKeyAvailable);
        }

        public void Dispose() => _polling.Dispose();

        public async ValueTask<KeyInput> ReadKeyAsync(CancellationToken token = default)
        {
            while (!token.IsCancellationRequested)
            {
                await _polling.PollAsync(token).ConfigureAwait(false);

                var consoleKey = _console.ReadKey(_intercept);
                bool hasMore = _console.IsKeyAvailable;
                var key = _keyConverter.ConvertKey(consoleKey.Key);
                var modifiers = _keyConverter.ConvertModifiers(consoleKey.Modifiers);

                return new KeyInput(new KeyInfo(key, modifiers), consoleKey.KeyChar, hasMore);
            }

            token.ThrowIfCancellationRequested();
            return default;
        }
    }
}
