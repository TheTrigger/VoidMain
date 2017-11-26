using Microsoft.Extensions.DependencyInjection;
using System;

namespace VoidMain.Hosting
{
    public interface ICommandsHostBuilder
    {
        ICommandsHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);
        ICommandsHost Build();
    }
}
