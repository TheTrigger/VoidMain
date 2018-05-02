using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace VoidMain.Application.Commands.Internal
{
    public class CachedTypeActivator : ITypeActivator
    {
        private readonly ConcurrentDictionary<Type, ObjectFactory> _cache =
               new ConcurrentDictionary<Type, ObjectFactory>();

        private readonly Func<Type, ObjectFactory> _factoryOfFactory =
            (implType) => ActivatorUtilities.CreateFactory(implType, Type.EmptyTypes);

        public object CreateInstance(Type implType, IServiceProvider services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (implType == null)
            {
                throw new ArgumentNullException(nameof(implType));
            }

            if (IsStatic(implType))
            {
                return null;
            }

            var factory = _cache.GetOrAdd(implType, _factoryOfFactory);
            var instance = factory(services, arguments: null);

            return instance;
        }

        private static bool IsStatic(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsAbstract && typeInfo.IsSealed;
        }
    }
}
