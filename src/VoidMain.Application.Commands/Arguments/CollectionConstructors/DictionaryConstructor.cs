using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors
{
    public class DictionaryConstructor : ICollectionConstructor
    {
        private static readonly Type[] SupportedTypes =
        {
            typeof(Dictionary<,>),
            typeof(IDictionary<,>),
            typeof(IReadOnlyDictionary<,>)
        };

        private static readonly Type DictionaryDefinition = typeof(Dictionary<,>);
        private static readonly Type KeyValyeDefinition = typeof(KeyValuePair<,>);

        private static readonly string KeyPropertyName = nameof(KeyValuePair<int, int>.Key);
        private static readonly string ValuePropertyName = nameof(KeyValuePair<int, int>.Value);

        public Type GetElementType(Type collectionType)
        {
            if (collectionType.IsParameterizedGeneric())
            {
                var definition = collectionType.GetGenericTypeDefinition();
                int index = Array.IndexOf(SupportedTypes, definition);
                if (index >= 0)
                {
                    return KeyValyeDefinition.MakeGenericType(collectionType.GenericTypeArguments);
                }
            }

            throw new NotSupportedException("Collection type must be a Dictionary<TKey, TValue> or one of its generic interfaces.");
        }

        public int GetElementsCount(object collection)
        {
            var dictionary = collection as IDictionary;
            if (dictionary == null)
            {
                throw new NotSupportedException("Collection must be a Dictionary<TKey, TValue>.");
            }

            return dictionary.Count;
        }

        public object Construct(Type elementType, object[] elements)
        {
            if (!elementType.IsParameterizedGeneric())
            {
                throw new NotSupportedException("Element type must be a KeyValuePair<TKey, Tvalue>.");
            }

            var definition = elementType.GetGenericTypeDefinition();
            if (definition != KeyValyeDefinition)
            {
                throw new NotSupportedException("Element type must be a KeyValuePair<TKey, Tvalue>.");
            }

            var keyValueParams = elementType.GenericTypeArguments;
            var dictionaryType = DictionaryDefinition.MakeGenericType(keyValueParams);
            var dictionary = (IDictionary)Activator.CreateInstance(dictionaryType, elements.Length);

            var typeInfo = elementType.GetTypeInfo();
            var keyProperty = typeInfo.GetProperty(KeyPropertyName);
            var valueProperty = typeInfo.GetProperty(ValuePropertyName);

            foreach (var element in elements)
            {
                var key = keyProperty.GetValue(element);
                var value = valueProperty.GetValue(element);
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        public void Deconstruct(object collection, object[] elements)
        {
            var dictionary = collection as IDictionary;
            if (dictionary == null)
            {
                throw new NotSupportedException("Collection must be a Dictionary<TKey, TValue>.");
            }

            dictionary.CopyTo(elements, 0);
        }
    }
}
