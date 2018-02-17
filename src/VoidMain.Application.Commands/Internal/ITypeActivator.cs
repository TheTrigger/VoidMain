using System;

namespace VoidMain.Application.Commands.Internal
{
    public interface ITypeActivator
    {
        object CreateInstance(IServiceProvider serviceProvider, Type implType);
    }
}
