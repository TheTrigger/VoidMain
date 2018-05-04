using System;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors
{
    public interface ICollectionConstructorProvider
    {
        bool IsCollection(Type collectionType);
        bool TryGetConstructor(Type collectionType, out ICollectionConstructor constructor);
    }
}
