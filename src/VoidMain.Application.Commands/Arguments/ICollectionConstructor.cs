using System;

namespace VoidMain.Application.Commands.Arguments
{
    public interface ICollectionConstructor
    {
        Type GetElementType(Type collectionType);
        ICollectionAdapter Create(Type elementType, int count);
        ICollectionAdapter Wrap(object collection);
    }
}
