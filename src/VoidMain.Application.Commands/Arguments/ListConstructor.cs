using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments
{
    public class ListConstructor : ICollectionConstructor
    {
        private readonly Type _listType = typeof(List<>);

        public Type GetElementType(Type collectionType)
        {
            if (!collectionType.GetTypeInfo().IsGenericType)
            {
                throw new AggregateException("Collection type must be generic.");
            }
            return collectionType.GenericTypeArguments[0];
        }

        public ICollectionAdapter Create(Type elementType, int count)
        {
            var specifiedListType = _listType.MakeGenericType(elementType);
            var list = (IList)Activator.CreateInstance(specifiedListType, (object)count);

            var defaultElementValue = elementType.GetEmptyValue();
            for (int i = 0; i < count; i++)
            {
                list.Add(defaultElementValue);
            }

            return new Adapter(list);
        }

        public ICollectionAdapter Wrap(object collection)
        {
            var list = collection as IList;
            if (list == null)
            {
                throw new ArgumentException("Collection must be a list.");
            }
            return new Adapter(list);
        }

        public class Adapter : ICollectionAdapter
        {
            private readonly IList _list;

            public object Collection => _list;
            public int Count => _list.Count;

            public Adapter(IList list)
            {
                _list = list;
            }

            public object GetValue(int index)
            {
                return _list[index];
            }

            public void SetValue(int index, object value)
            {
                _list[index] = value;
            }
        }
    }
}
