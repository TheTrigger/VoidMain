using System;
using System.Reflection;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.CollectionConstructors
{
    public class CollectionConstructorProvider : ICollectionConstructorProvider
    {
        private readonly ITypeActivator _typeActivator;
        private readonly IServiceProvider _services;
        private readonly CollectionConstructorProviderOptions _options;

        public CollectionConstructorProvider(
            ITypeActivator typeActivator,
            IServiceProvider services,
            CollectionConstructorProviderOptions options = null)
        {
            _typeActivator = typeActivator ?? throw new ArgumentNullException(nameof(typeActivator));
            _services = services ?? throw new ArgumentNullException(nameof(services));
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
            if (_options.CollectionConstructors.TryGetValue(collectionType, out var config))
            {
                constructor = GetInstance(config, _services);
                return true;
            }

            if (collectionType.IsArray)
            {
                constructor = _options.ArrayConstructor != null
                    ? GetInstance(_options.ArrayConstructor, _services)
                    : null;
                return constructor != null;
            }

            if (collectionType.GetTypeInfo().IsGenericType)
            {
                var definition = collectionType.GetGenericTypeDefinition();
                if (_options.CollectionConstructors.TryGetValue(definition, out config))
                {
                    constructor = GetInstance(config, _services);
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
