using System;
using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public class ConsoleKeyReader : IInputKeyReader
    {
        private const int FAST_POLLING_TIME = 15;
        private const int FAST_POLLING_DURATION = 500;
        private const int NORMAL_POLLING_DURATION = 10_000;

        private readonly IConsole _console;
        private readonly IConsoleKeyConverter _keyConverter;
        private readonly int _normalPollingTime;
        private readonly int _slowPollingTime;
        private readonly int _maxNormalAttempts;
        private readonly int _fastPollingTime;
        private readonly int _maxFastAttempts;

        public ConsoleKeyReader(
            IConsole console,
            IConsoleKeyConverter keyConverter,
            ConsoleKeyReaderOptions options = null)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _keyConverter = keyConverter ?? throw new ArgumentNullException(nameof(keyConverter));
            if (options == null)
            {
                options = new ConsoleKeyReaderOptions();
            }
            options.Validate();

            _fastPollingTime = FAST_POLLING_TIME;
            _maxFastAttempts = FAST_POLLING_DURATION / _fastPollingTime;

            _normalPollingTime = options.PollingTime;
            _maxNormalAttempts = _maxFastAttempts + NORMAL_POLLING_DURATION / _normalPollingTime;

            _slowPollingTime = options.MaxPollingTime;
        }

        public async Task<InputKeyInfo> ReadKeyAsync(bool intercept = false, CancellationToken token = default)
        {
            int pollingTime = _fastPollingTime;
            int attempts = 0;

            while (!token.IsCancellationRequested)
            {
                if (_console.IsKeyAvailable)
                {
                    var keyInfo = _console.ReadKey(intercept);
                    bool hasMoreInput = _console.IsKeyAvailable;
                    var key = _keyConverter.ConvertKey(keyInfo.Key);
                    var modifiers = _keyConverter.ConvertModifiers(keyInfo.Modifiers);

                    return new InputKeyInfo(key, modifiers, keyInfo.KeyChar, hasMoreInput);
                }

                await Task.Delay(pollingTime, token);

                if (attempts == _maxFastAttempts)
                {
                    pollingTime = _normalPollingTime;
                }
                else if (attempts == _maxNormalAttempts)
                {
                    pollingTime = _slowPollingTime;
                }

                if (attempts <= _maxNormalAttempts)
                {
                    attempts++;
                }
            }

            token.ThrowIfCancellationRequested();
            return default(InputKeyInfo);
        }
    }
}
