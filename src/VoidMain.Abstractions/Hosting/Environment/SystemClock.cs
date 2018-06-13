using System;

namespace VoidMain.Hosting.Environment
{
    public class SystemClock : IClock
    {
        public DateTime Now() => DateTime.Now;
        public DateTime UtcNow() => DateTime.UtcNow;
    }
}
