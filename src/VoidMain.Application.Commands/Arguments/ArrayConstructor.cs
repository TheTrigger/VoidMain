using System;
using System.Reflection;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArrayConstructor : ICollectionConstructor
    {
        public Type GetElementType(Type collectionType)
        {
            if (collectionType.IsArray)
            {
                return collectionType.GetElementType();
            }
            if (collectionType.GetTypeInfo().IsGenericType)
            {
                return collectionType.GenericTypeArguments[0];
            }

            throw new ArgumentException("Collection type must be an array or one of it's generic interfaces.");
        }

        public ICollectionAdapter Create(Type elementType, int count)
        {
            var array = Array.CreateInstance(elementType, count);
            return new Adapter(array);
        }

        public ICollectionAdapter Wrap(object collection)
        {
            var array = collection as Array;
            if (array == null)
            {
                throw new ArgumentException("Collection must be an array.");
            }
            return new Adapter(array);
        }

        public class Adapter : ICollectionAdapter
        {
            private readonly Array _array;

            public object Collection => _array;
            public int Count => _array.Length;

            public Adapter(Array array)
            {
                _array = array;
            }

            public object GetValue(int index)
            {
                return _array.GetValue(index);
            }

            public void SetValue(int index, object value)
            {
                _array.SetValue(value, index);
            }
        }
    }
}
