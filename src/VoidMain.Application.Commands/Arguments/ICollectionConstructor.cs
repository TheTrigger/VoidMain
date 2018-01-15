using System;

namespace VoidMain.Application.Commands.Arguments
{
    public interface ICollectionConstructor
    {
        Type GetElementType(Type collectionType);
        ICollectionInitializer Create(Type elementType, int count);
    }
}
