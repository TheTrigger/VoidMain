using System;

namespace VoidMain.Application.Commands.Execution
{
    public interface IModuleInstanceFactory
    {
        object Create(Type moduleType, IServiceProvider services);
    }
}
