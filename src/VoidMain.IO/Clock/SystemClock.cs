using System;
using System.Diagnostics;

namespace VoidMain.IO.Clock
{
    public class SystemClock : IClock
    {
        public DateTime Now() => DateTime.Now;
        public DateTime UtcNow() => DateTime.UtcNow;
        public long GetTimestamp() => Stopwatch.GetTimestamp();
    }
}
