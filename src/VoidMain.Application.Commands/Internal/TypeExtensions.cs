using System;
using System.Reflection;

namespace VoidMain.Application.Commands.Internal
{
    public static class TypeExtensions
    {
        private static readonly TypeInfo DisposableType = typeof(IDisposable).GetTypeInfo();
        private static readonly string DisposableName = nameof(IDisposable.Dispose);

        public static bool IsNullable(this Type type)
        {
            if (!type.GetTypeInfo().IsValueType) return false;
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static Type UnwrapIfNullable(this Type type)
        {
            if (type.IsNullable())
            {
                return Nullable.GetUnderlyingType(type);
            }
            return type;
        }

        public static bool IsDbNull(this Type type)
        {
            return type.FullName == "System.DBNull";
        }

        public static bool IsObjectBaseClassMethod(this MethodInfo method)
        {
            return method.GetBaseDefinition().DeclaringType == typeof(Object);
        }

        public static bool IsDisposeMethod(this MethodInfo method)
        {
            return method.Name == DisposableName
                && DisposableType.IsAssignableFrom(method.DeclaringType);
        }

        public static object GetEmptyValue(this Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
