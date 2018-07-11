using System;

namespace VoidMain.CommandLineIinterface.IO.Console
{
    public class ConsoleKeyReaderOptions
    {
        public int PollingTime { get; set; }
        public int MaxPollingTime { get; set; }

        public ConsoleKeyReaderOptions(bool defaults = true)
        {
            if (defaults)
            {
                PollingTime = 100;
                MaxPollingTime = 1000;
            }
        }

        public void Validate()
        {
            if (PollingTime < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(PollingTime));
            }
            if (MaxPollingTime < PollingTime)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxPollingTime));
            }
        }
    }
}
