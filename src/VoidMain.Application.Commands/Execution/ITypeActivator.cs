using System;

namespace VoidMain.Application.Commands.Execution
{
    public interface ITypeActivator
    {
        object CreateInstance(IServiceProvider serviceProvider, Type implType);
    }
}
