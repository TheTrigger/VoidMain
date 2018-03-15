using System;

namespace VoidMain.Application.Commands.Arguments
{
    public interface ICollectionConstructorProvider
    {
        bool IsCollection(Type collectionType);
        bool TryGetCollectionConstructor(Type collectionType, out ICollectionConstructor constructor);
    }
}
