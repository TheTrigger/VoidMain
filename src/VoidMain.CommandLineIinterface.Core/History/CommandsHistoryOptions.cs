using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryOptions
    {
        public int MaxCount { get; set; }
        public TimeSpan SavePeriod { get; set; }
        public IEqualityComparer<string> CommandsComparer { get; set; }

        public CommandsHistoryOptions(bool defaults = true)
        {
            if (defaults)
            {
                MaxCount = 10;
                SavePeriod = TimeSpan.FromSeconds(10.0);
                CommandsComparer = StringComparer.Ordinal;
            }
        }

        public void Validate()
        {
            if (MaxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxCount));
            }
            if (SavePeriod < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(SavePeriod));
            }
            if (CommandsComparer == null)
            {
                throw new ArgumentNullException(nameof(CommandsComparer));
            }
        }
    }
}
