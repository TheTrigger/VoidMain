using System;
using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineIinterface.IO.Console.Internal
{
    public class ConsoleKeyAsyncReader
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

        public ConsoleKeyAsyncReader(IConsole console, int pollingTime, int maxPollingTime)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));

            if (pollingTime < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pollingTime));
            }
            if (maxPollingTime < pollingTime)
            {
                throw new ArgumentOutOfRangeException(nameof(maxPollingTime));
            }

            _fastPollingTime = FAST_POLLING_TIME;
            _maxFastAttempts = FAST_POLLING_DURATION / _fastPollingTime;

            _normalPollingTime = pollingTime;
            _maxNormalAttempts = _maxFastAttempts + NORMAL_POLLING_DURATION / _normalPollingTime;

            _slowPollingTime = maxPollingTime;
        }

        public async Task<AdvancedConsoleKeyInfo> ReadKeyAsync(CancellationToken token, bool intercept = false)
        {
            int pollingTime = _fastPollingTime;
            int attempts = 0;

            while (!token.IsCancellationRequested)
            {
                if (_console.KeyAvailable)
                {
                    var keyInfo =_console.ReadKey(intercept);
                    bool isNextKeyAvailable = _console.KeyAvailable;
                    return new AdvancedConsoleKeyInfo(keyInfo, isNextKeyAvailable);
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
            return default(AdvancedConsoleKeyInfo);
        }
    }
}
