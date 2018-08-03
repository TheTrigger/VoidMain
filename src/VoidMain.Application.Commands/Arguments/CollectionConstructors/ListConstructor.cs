using System;
using System.Collections;
using System.Collections.Generic;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors
{
    public class ListConstructor : ICollectionConstructor
    {
        private static readonly Type[] SupportedTypes =
        {
            typeof(List<>),
            typeof(IEnumerable<>),
            typeof(ICollection<>),
            typeof(IReadOnlyCollection<>),
            typeof(IList<>),
            typeof(IReadOnlyList<>)
        };

        private static readonly Type ListDefinition = typeof(List<>);

        public Type GetElementType(Type collectionType)
        {
            if (collectionType.IsParameterizedGeneric())
            {
                var definition = collectionType.GetGenericTypeDefinition();
                int index = Array.IndexOf(SupportedTypes, definition);
                if (index >= 0)
                {
                    return collectionType.GenericTypeArguments[0];
                }
            }

            throw new NotSupportedException("Collection type must be a List<T> or one of its generic interfaces.");
        }

        public int GetElementsCount(object collection)
        {
            var list = collection as IList;
            if (list == null)
            {
                throw new NotSupportedException("Collection must be a List<T>.");
            }

            return list.Count;
        }

        public object Construct(Type elementType, object[] elements)
        {
            var listType = ListDefinition.MakeGenericType(elementType);
            var list = (IList)Activator.CreateInstance(listType, elements.Length);
            foreach (var element in elements)
            {
                list.Add(element);
            }

            return list;
        }

        public void Deconstruct(object collection, object[] elements)
        {
            var list = collection as IList;
            if (list == null)
            {
                throw new NotSupportedException("Collection must be a List<T>.");
            }

            list.CopyTo(elements, 0);
        }
    }
}
