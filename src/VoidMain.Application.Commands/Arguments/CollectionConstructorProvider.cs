using System;
using System.Reflection;

namespace VoidMain.Application.Commands.Arguments
{
    public class CollectionConstructorProvider : ICollectionConstructorProvider
    {
        private readonly CollectionConstructorProviderOptions _options;

        public CollectionConstructorProvider(CollectionConstructorProviderOptions options = null)
        {
            _options = options ?? new CollectionConstructorProviderOptions();
            _options.Validate();
        }

        public bool IsCollection(Type collectionType)
        {
            if (_options.CollectionConstructors.ContainsKey(collectionType))
            {
                return true;
            }

            if (collectionType.IsArray)
            {
                return _options.ArrayConstructor != null;
            }

            if (!collectionType.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            var genericDefinition = collectionType.GetGenericTypeDefinition();
            return _options.CollectionConstructors.ContainsKey(genericDefinition);
        }

        public bool TryGetConstructor(Type collectionType, out ICollectionConstructor constructor)
        {
            if (_options.CollectionConstructors.TryGetValue(collectionType, out constructor))
            {
                return true;
            }

            if (collectionType.IsArray)
            {
                constructor = _options.ArrayConstructor;
                return constructor != null;
            }

            if (!collectionType.GetTypeInfo().IsGenericType)
            {
                constructor = null;
                return false;
            }

            var genericDefinition = collectionType.GetGenericTypeDefinition();
            return _options.CollectionConstructors.TryGetValue(genericDefinition, out constructor);
        }
    }
}
