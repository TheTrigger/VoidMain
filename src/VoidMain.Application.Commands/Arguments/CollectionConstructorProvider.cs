using System;
using System.Collections.Generic;
using System.Reflection;

namespace VoidMain.Application.Commands.Arguments
{
    public class CollectionConstructorProvider : ICollectionConstructorProvider
    {
        private readonly ICollectionConstructor _arrayCtor;
        private readonly Dictionary<Type, ICollectionConstructor> _colCtors;

        public CollectionConstructorProvider(CollectionConstructorProviderOptions options = null)
        {
            _arrayCtor = options?.ArrayConstructor ?? new ArrayConstructor();
            _colCtors = InitConstructors(options?.CollectionConstructors);
        }

        private Dictionary<Type, ICollectionConstructor> InitConstructors(
            Dictionary<Type, ICollectionConstructor> customCtors)
        {
            var ctors = new Dictionary<Type, ICollectionConstructor>();

            // Default ctors.
            ctors[typeof(IEnumerable<>)] = _arrayCtor;
            ctors[typeof(ICollection<>)] = _arrayCtor;
            ctors[typeof(IReadOnlyCollection<>)] = _arrayCtor;
            ctors[typeof(IReadOnlyList<>)] = _arrayCtor;
            var listCtor = new ListConstructor();
            ctors[typeof(IList<>)] = listCtor;
            ctors[typeof(List<>)] = listCtor;

            // Custom ctors.
            if (customCtors?.Count > 0)
            {
                foreach (var custom in customCtors)
                {
                    if (custom.Value == null)
                    {
                        ctors.Remove(custom.Key);
                    }
                    else
                    {
                        ctors[custom.Key] = custom.Value;
                    }
                }
            }

            return ctors;
        }

        public bool IsCollection(Type collectionType)
        {
            if (_colCtors.ContainsKey(collectionType))
            {
                return true;
            }

            if (collectionType.IsArray)
            {
                return _arrayCtor != null;
            }

            if (!collectionType.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            var genericDefinition = collectionType.GetGenericTypeDefinition();
            return _colCtors.ContainsKey(genericDefinition);
        }

        public bool TryGetConstructor(Type collectionType, out ICollectionConstructor constructor)
        {
            if (_colCtors.TryGetValue(collectionType, out constructor))
            {
                return true;
            }

            if (collectionType.IsArray)
            {
                constructor = _arrayCtor;
                return constructor != null;
            }

            if (!collectionType.GetTypeInfo().IsGenericType)
            {
                constructor = null;
                return false;
            }

            var genericDefinition = collectionType.GetGenericTypeDefinition();
            return _colCtors.TryGetValue(genericDefinition, out constructor);
        }
    }
}
