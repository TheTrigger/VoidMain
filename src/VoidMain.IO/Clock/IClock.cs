using System;

namespace VoidMain.IO.Clock
{
    public interface IClock
    {
        DateTime Now();
        DateTime UtcNow();
        long GetTimestamp();
    }
}
