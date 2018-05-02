using System;
using System.Diagnostics;
using System.Reflection;

namespace VoidMain
{
    [DebuggerDisplay("{" + nameof(GetImplType) + "()}")]
    public class TypeOrInstance<TInstance>
        where TInstance : class
    {
        private static TypeInfo Constraint = typeof(TInstance).GetTypeInfo();

        public TInstance Instance { get; }
        public Type Type { get; }

        public Type GetImplType() => Instance?.GetType() ?? Type;

        public TypeOrInstance(TInstance instance)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public TypeOrInstance(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            if (!Constraint.IsAssignableFrom(Type))
            {
                throw new ArgumentException($"Type `{Type.Name}` is not assignable to type `{Constraint.Name}`.");
            }
        }

        public override string ToString() => GetImplType().Name;

        public static implicit operator TypeOrInstance<TInstance>(TInstance instance)
        {
            return new TypeOrInstance<TInstance>(instance);
        }

        public static implicit operator TypeOrInstance<TInstance>(Type type)
        {
            return new TypeOrInstance<TInstance>(type);
        }
    }
}
