using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace VoidMain.Application.Commands.Arguments
{
    public class ListConstructor : ICollectionConstructor
    {
        private readonly Type _listType = typeof(List<>);

        public Type GetElementType(Type collectionType)
        {
            return collectionType.GenericTypeArguments[0];
        }

        public ICollectionInitializer Create(Type elementType, int count)
        {
            var specifiedListType = _listType.MakeGenericType(elementType);
            var list = (IList)Activator.CreateInstance(specifiedListType, (object)count);
            var defaultElementValue = elementType.GetTypeInfo().IsClass
                ? null
                : Activator.CreateInstance(elementType);
            for (int i = 0; i < count; i++)
            {
                list.Add(defaultElementValue);
            }
            return new Initializer(list);
        }

        public class Initializer : ICollectionInitializer
        {
            private readonly IList _list;
            public object Collection => _list;

            public Initializer(IList list)
            {
                _list = list;
            }

            public void SetValue(int index, object value)
            {
                _list[index] = value;
            }
        }
    }
}
