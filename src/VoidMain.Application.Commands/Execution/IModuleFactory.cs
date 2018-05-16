using System;

namespace VoidMain.Application.Commands.Execution
{
    public interface IModuleFactory
    {
        object Create(Type moduleType, IServiceProvider services);
    }
}
