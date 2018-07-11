using System;
using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public class ConsoleKeyReader : IConsoleKeyReader
    {
        private const int FAST_POLLING_TIME = 15;
        private const int FAST_POLLING_DURATION = 500;
        private const int NORMAL_POLLING_DURATION = 10_000;

        private readonly IConsole _console;
        private readonly int _normalPollingTime;
        private readonly int _slowPollingTime;
        private readonly int _maxNormalAttempts;
        private readonly int _fastPollingTime;
        private readonly int _maxFastAttempts;

        public ConsoleKeyReader(IConsole console, ConsoleKeyReaderOptions options = null)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
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

        public async Task<ExtendedConsoleKeyInfo> ReadKeyAsync(
            bool intercept = false, CancellationToken token = default(CancellationToken))
        {
            int pollingTime = _fastPollingTime;
            int attempts = 0;

            while (!token.IsCancellationRequested)
            {
                if (_console.KeyAvailable)
                {
                    var keyInfo = _console.ReadKey(intercept);
                    bool isNextKeyAvailable = _console.KeyAvailable;
                    return new ExtendedConsoleKeyInfo(keyInfo, isNextKeyAvailable);
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
            return default(ExtendedConsoleKeyInfo);
        }
    }
}
