using System;
using System.Reflection;
using System.Threading;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public class ArgumentModelConstructor : IArgumentModelConstructor
    {
        private readonly Type TokenType = typeof(CancellationToken);
        private readonly ICollectionConstructorProvider _colCtorProvider;

        public ArgumentModelConstructor(ICollectionConstructorProvider colCtorProvider)
        {
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
        }

        public ArgumentModel Create(ParameterInfo parameter, CommandModel command)
        {
            var paramType = parameter.ParameterType;

            var argument = new ArgumentModel();
            argument.Parameter = parameter;
            argument.Type = paramType;
            argument.Name = parameter.Name;

            var attr = parameter.GetCustomAttribute<ParameterAttribute>();
            bool isNullableOrParams = paramType.IsNullable() ||
                parameter.IsDefined(typeof(ParamArrayAttribute));

            if (attr == null)
            {
                argument.Kind = GetKindFromType(paramType);
                argument.DefaultValue = GetDefaultValue(parameter);
                argument.Optional = parameter.IsOptional || isNullableOrParams;
            }
            else
            {
                argument.Kind = GetKindFromAttribute(attr);
                if (attr is ArgumentAttribute argAttr)
                {
                    if (argAttr.Name != null)
                    {
                        argument.Name = argAttr.Name;
                    }
                    if (argAttr.IsAliasSet)
                    {
                        argument.Alias = argAttr.Alias.ToString();
                    }
                    argument.Description = argAttr.Description;
                    argument.DefaultValue = argAttr.DefaultValue ?? GetDefaultValue(parameter);
                    argument.ValueParser = argAttr.ValueParser;
                }

                argument.Optional = attr.IsSetOptional
                    ? attr.Optional
                    : parameter.IsOptional || isNullableOrParams || argument.DefaultValue != null;
            }

            return argument;
        }

        private ArgumentKind GetKindFromType(Type paramType)
        {
            if (_colCtorProvider.IsCollection(paramType))
            {
                return ArgumentKind.Operand;
            }

            if (paramType.GetTypeInfo().IsInterface || paramType == TokenType)
            {
                return ArgumentKind.Service;
            }

            return ArgumentKind.Option;
        }

        private ArgumentKind GetKindFromAttribute(ParameterAttribute attr)
        {
            if (attr is OperandAttribute)
            {
                return ArgumentKind.Operand;
            }
            if (attr is ServiceAttribute)
            {
                return ArgumentKind.Service;
            }
            return ArgumentKind.Option;
        }

        private object GetDefaultValue(ParameterInfo parameter)
        {
            var value = parameter.DefaultValue;
            if (ReferenceEquals(value, null)) return value;
            if (value.GetType().IsDbNull()) return null;
            return value;
        }
    }
}
