using System;

namespace VoidMain.IO.Console
{
    public class ConsoleKeyReaderOptions
    {
        public bool Intercept { get; set; } = true;
        public TimeSpan PollingPeriod { get; set; } = TimeSpan.FromMilliseconds(50.0);
    }
}
