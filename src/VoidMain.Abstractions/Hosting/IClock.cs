using System;

namespace VoidMain.Hosting
{
    public interface IClock
    {
        DateTime Now();
        DateTime UtcNow();
    }
}
