using System;
using System.Reflection;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments
{
    public class CollectionConstructorProvider : ICollectionConstructorProvider
    {
        private readonly ITypeActivator _typeActivator;
        private readonly CollectionConstructorProviderOptions _options;

        public CollectionConstructorProvider(
            ITypeActivator typeActivator,
            CollectionConstructorProviderOptions options = null)
        {
            _typeActivator = typeActivator ?? throw new ArgumentNullException(nameof(typeActivator));
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

        public bool TryGetConstructor(Type collectionType,
            IServiceProvider services, out ICollectionConstructor constructor)
        {
            if (_options.CollectionConstructors.TryGetValue(collectionType, out var config))
            {
                constructor = GetInstance(config, services);
                return true;
            }

            if (collectionType.IsArray)
            {
                constructor = GetInstance(_options.ArrayConstructor, services);
                return true;
            }

            if (collectionType.GetTypeInfo().IsGenericType)
            {
                var genericDefinition = collectionType.GetGenericTypeDefinition();
                if (_options.CollectionConstructors.TryGetValue(genericDefinition, out config))
                {
                    constructor = GetInstance(config, services);
                    return true;
                }
            }

            constructor = null;
            return false;
        }

        private ICollectionConstructor GetInstance(Type parserType, IServiceProvider services)
        {
            return (ICollectionConstructor)_typeActivator.CreateInstance(parserType, services);
        }

        private ICollectionConstructor GetInstance(TypeOrInstance<ICollectionConstructor> config, IServiceProvider services)
        {
            return config.Instance ?? GetInstance(config.Type, services);
        }
    }
}
