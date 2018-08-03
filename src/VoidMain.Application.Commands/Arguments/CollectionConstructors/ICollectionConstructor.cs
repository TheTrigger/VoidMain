using System;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors
{
    public interface ICollectionConstructor
    {
        Type GetElementType(Type collectionType);
        int GetElementsCount(object collection);
        object Construct(Type elementType, object[] elements);
        void Deconstruct(object collection, object[] elements);
    }
}
