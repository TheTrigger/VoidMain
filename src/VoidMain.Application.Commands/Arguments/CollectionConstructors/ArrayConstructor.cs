using System;
using System.Collections.Generic;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors
{
    public class ArrayConstructor : ICollectionConstructor
    {
        private static readonly Type[] SupportedTypes =
        {
            typeof(IEnumerable<>),
            typeof(ICollection<>),
            typeof(IReadOnlyCollection<>),
            typeof(IList<>),
            typeof(IReadOnlyList<>)
        };

        public Type GetElementType(Type collectionType)
        {
            if (collectionType.IsArray)
            {
                return collectionType.GetElementType();
            }

            if (collectionType.IsParameterizedGeneric())
            {
                var definition = collectionType.GetGenericTypeDefinition();
                int index = Array.IndexOf(SupportedTypes, definition);
                if (index >= 0)
                {
                    return collectionType.GenericTypeArguments[0];
                }
            }

            throw new NotSupportedException("Collection type must be an Array or one of its generic interfaces.");
        }

        public int GetElementsCount(object collection)
        {
            var array = collection as Array;
            if (array == null)
            {
                throw new NotSupportedException("Collection must be an Array.");
            }

            return array.GetLength(0);
        }

        public object Construct(Type elementType, object[] elements)
        {
            var array = Array.CreateInstance(elementType, elements.Length);
            Array.Copy(elements, array, elements.Length);
            return array;
        }

        public void Deconstruct(object collection, object[] elements)
        {
            var array = collection as Array;
            if (array == null)
            {
                throw new NotSupportedException("Collection must be an Array.");
            }

            Array.Copy(array, elements, array.Length);
        }
    }
}
