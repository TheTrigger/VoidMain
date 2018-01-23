using System;

namespace VoidMain.Application.Commands.Arguments
{
    public interface ICollectionConstructorProvider
    {
        bool TryGetCollectionConstructor(Type collectionType, out ICollectionConstructor constructor);
    }
}
