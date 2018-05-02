using System;

namespace VoidMain.Application.Commands.Internal
{
    public interface ITypeActivator
    {
        object CreateInstance(Type implType, IServiceProvider services);
    }
}
