using System;
using System.Reflection;
using System.Text;

namespace VoidMain.Application.Commands.Internal
{
    public static class TypeExtensions
    {
        private static readonly TypeInfo DisposableType = typeof(IDisposable).GetTypeInfo();
        private static readonly string DisposableName = nameof(IDisposable.Dispose);
        private static readonly Type ObjectType = typeof(Object);

        public static bool IsNullable(this Type type)
        {
            if (!type.GetTypeInfo().IsValueType)
            {
                return false;
            }
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

        public static bool IsParameterizedGeneric(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsGenericType
                && !typeInfo.ContainsGenericParameters;
        }

        public static bool IsParams(this ParameterInfo param)
        {
            return param.IsDefined(typeof(ParamArrayAttribute));
        }

        public static bool IsObjectBaseClassMethod(this MethodInfo method)
        {
            return method.GetBaseDefinition().DeclaringType == ObjectType;
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

        public static string ToDisplayString(this MethodInfo method)
        {
            var result = new StringBuilder();

            result.Append(method.ReturnType.Name)
                .Append(' ')
                .Append(method.DeclaringType.Name)
                .Append('.')
                .Append(method.Name)
                .Append('(');

            var parameters = method.GetParameters();
            foreach (var param in parameters)
            {
                result.Append(param.ParameterType.Name)
                    .Append(' ')
                    .Append(param.Name)
                    .Append(", ");
            }

            if (parameters.Length > 0)
            {
                result.Length -= 2;
            }

            result.Append(')');

            return result.ToString();
        }
    }
}
