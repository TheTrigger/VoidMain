using System;
using System.Reflection;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public class ArgumentModelConstructor : IArgumentModelConstructor
    {
        public ArgumentModel Create(ParameterInfo parameter, CommandModel command)
        {
            var paramType = parameter.ParameterType;

            var argument = new ArgumentModel();
            argument.Parameter = parameter;
            argument.Type = paramType;

            var attr = parameter.GetCustomAttribute<ParameterAttribute>();
            bool isNullableOrParams = paramType.IsNullable() ||
                parameter.IsDefined(typeof(ParamArrayAttribute));

            if (attr == null)
            {
                argument.Kind = GetKindFromType(paramType);
                argument.Name = parameter.Name;
                argument.DefaultValue = GetDefaultValue(parameter);
                argument.Optional = parameter.IsOptional || isNullableOrParams;
            }
            else
            {
                argument.Kind = GetKindFromAttributeOrType(attr, paramType);
                if (attr is ArgumentAttribute argAttr)
                {
                    argument.Name = argAttr.Name ?? parameter.Name;
                    if (argAttr.IsAliasSet)
                    {
                        argument.Alias = argAttr.Alias.ToString();
                    }
                    argument.Description = argAttr.Description;
                    argument.DefaultValue = argAttr.DefaultValue ?? GetDefaultValue(parameter);
                    argument.ValueParser = argAttr.ValueParser;
                }
                argument.Optional = attr.IsOptionalSet
                    ? attr.Optional
                    : parameter.IsOptional || isNullableOrParams || argument.DefaultValue != null;
            }

            return argument;
        }

        private ArgumentKind GetKindFromType(Type paramType)
        {
            if (paramType.GetTypeInfo().IsInterface)
            {
                return ArgumentKind.Service;
            }
            if (paramType.IsArray)
            {
                return ArgumentKind.Operand;
            }
            return ArgumentKind.Option;
        }

        private ArgumentKind GetKindFromAttributeOrType(ParameterAttribute attr, Type paramType)
        {
            if (attr is ServiceAttribute)
            {
                return ArgumentKind.Service;
            }
            if (attr is OptionAttribute)
            {
                return ArgumentKind.Option;
            }
            if (attr is OperandAttribute)
            {
                return ArgumentKind.Operand;
            }
            return GetKindFromType(paramType);
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
