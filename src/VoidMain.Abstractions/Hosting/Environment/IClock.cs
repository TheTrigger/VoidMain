using System;

namespace VoidMain.Hosting.Environment
{
    public interface IClock
    {
        DateTime Now();
        DateTime UtcNow();
    }
}
