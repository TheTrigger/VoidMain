using Microsoft.Extensions.DependencyInjection;
using System;

namespace VoidMain.Hosting.Tests.Tools
{
    public static class Services
    {
        private readonly static Lazy<IServiceProvider> _instance = new Lazy<IServiceProvider>(
            () => new ServiceCollection().BuildServiceProvider());
        public static IServiceProvider Empty => _instance.Value;
    }
}
