using System;

namespace VoidMain.IO.Clock
{
    public class SystemClock : IClock
    {
        public DateTime Now() => DateTime.Now;
        public DateTime UtcNow() => DateTime.UtcNow;
    }
}
