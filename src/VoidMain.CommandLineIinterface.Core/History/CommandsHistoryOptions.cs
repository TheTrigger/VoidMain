using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryOptions
    {
        public int? MaxCount { get; set; }
        public TimeSpan? SavePeriod { get; set; }
        public IEqualityComparer<string> CommandsComparer { get; set; }
    }
}
