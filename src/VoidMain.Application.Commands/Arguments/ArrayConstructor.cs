using System;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArrayConstructor : ICollectionConstructor
    {
        public Type GetElementType(Type collectionType)
        {
            return collectionType.GetElementType();
        }

        public ICollectionInitializer Create(Type elementType, int count)
        {
            var array = Array.CreateInstance(elementType, count);
            return new Initializer(array);
        }

        public class Initializer : ICollectionInitializer
        {
            private readonly Array _array;
            public object Collection => _array;

            public Initializer(Array array)
            {
                _array = array;
            }

            public void SetValue(int index, object value)
            {
                _array.SetValue(value, index);
            }
        }
    }
}
