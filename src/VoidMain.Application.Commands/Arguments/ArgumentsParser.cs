using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParser : IArgumentsParser
    {
        private readonly Type StringType = typeof(string);

        private readonly IServiceProvider _services;
        private readonly ICollectionConstructorProvider _colCtorProvider;
        private readonly IValueParserProvider _parserProvider;
        private readonly ICollectionConstructor _arrayCtor;
        private readonly IFormatProvider _formatProvider;

        public ArgumentsParser(IServiceProvider services,
            ICollectionConstructorProvider colCtorProvider,
            IValueParserProvider parserProvider,
            ArgumentsParserOptions options = null)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
            _parserProvider = parserProvider ?? throw new ArgumentNullException(nameof(parserProvider));
            if (!_colCtorProvider.TryGetCollectionConstructor(typeof(Array), out _arrayCtor))
            {
                _arrayCtor = new ArrayConstructor();
            }
            _formatProvider = options?.FormatProvider ?? CultureInfo.CurrentCulture;

        }

        public object[] Parse(
            IReadOnlyList<ArgumentModel> argsModel,
            Dictionary<string, string[]> options, string[] operands)
        {
            if (argsModel == null)
            {
                throw new ArgumentNullException(nameof(argsModel));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (operands == null)
            {
                throw new ArgumentNullException(nameof(operands));
            }

            var values = new object[argsModel.Count];
            int operandsOffset = 0;

            for (int i = 0; i < argsModel.Count; i++)
            {
                var arg = argsModel[i];

                switch (arg.Kind)
                {
                    case ArgumentKind.Service:
                        values[i] = GetService(arg);
                        break;
                    case ArgumentKind.Option:
                        string[] optionValues = GetOptionValues(options, arg);
                        values[i] = ParseValueOrGetDefault(arg, optionValues, 0, out int _, useLastValue: true);
                        break;
                    case ArgumentKind.Operand:
                        values[i] = ParseValueOrGetDefault(arg, operands, operandsOffset, out int operandsUsed);
                        operandsOffset += operandsUsed;
                        break;
                    default:
                        throw new ArgumentParseException(arg, $"Unknown argument kind '{arg.Kind}'.");
                }
            }

            return values;
        }

        private string[] GetOptionValues(Dictionary<string, string[]> options, ArgumentModel arg)
        {
            if (!options.TryGetValue(arg.Name, out string[] values))
            {
                if (arg.Alias != null)
                {
                    options.TryGetValue(arg.Alias, out values);
                }
            }
            return values;
        }

        private object GetService(ArgumentModel arg)
        {
            var service = _services.GetService(arg.Type);
            if (service == null)
            {
                if (arg.Optional)
                {
                    return GetEmptyValue(arg.Type);
                }

                throw new ArgumentParseException(arg, $"Service '{arg.Type}' is not registered.");
            }
            return service;
        }

        private object ParseValueOrGetDefault(ArgumentModel arg, string[] values,
            int valuesOffset, out int valuesUsed, bool useLastValue = false)
        {
            bool isDefaultValue = false;

            if (values == null || valuesOffset == values.Length)
            {
                var defaultValue = arg.DefaultValue;
                if (defaultValue == null)
                {
                    if (arg.Optional)
                    {
                        valuesUsed = 0;
                        return GetEmptyValue(arg.Type);
                    }

                    throw new ArgumentParseException(arg, "Value is missing.");
                    // TODO: Or prompt value.
                }

                var argType = arg.Type.UnwrapIfNullable();
                var defaultValueType = defaultValue.GetType();

                if (argType == defaultValueType)
                {
                    valuesUsed = 0;
                    if (argType.IsArray)
                    {
                        var source = (Array)defaultValue;
                        return CopyArray(source, 0, source.Length);
                    }
                    return defaultValue;
                }

                switch (defaultValue)
                {
                    case string stringValue:
                        values = new[] { stringValue };
                        break;
                    case string[] stringValues:
                        if (stringValues.Length == 0)
                        {
                            // Because argType != defaultValueType
                            // and we can't parse an empty string array.
                            throw new ArgumentParseException(arg, "Types of the argument and default value do not match.");
                        }
                        values = stringValues;
                        break;
                    default:
                        throw new ArgumentParseException(arg, "Types of the argument and default value do not match.");
                }

                valuesOffset = 0;
                isDefaultValue = true;
            }

            var value = ParseValue(arg, values, valuesOffset, out valuesUsed, useLastValue);
            if (isDefaultValue)
            {
                valuesUsed = 0;
            }
            return value;
        }

        private object ParseValue(ArgumentModel arg, string[] stringValues,
            int valuesOffset, out int valuesUsed, bool useLastValue = false)
        {
            var argType = arg.Type;

            if (argType.IsArray && argType.GetElementType() == StringType)
            {
                valuesUsed = stringValues.Length - valuesOffset;
                var values = CopyArray(stringValues, valuesOffset, valuesUsed);
                return values;
            }

            bool isCollection = _colCtorProvider.TryGetCollectionConstructor(
                argType, out ICollectionConstructor colCtor);

            if (isCollection)
            {
                valuesUsed = stringValues.Length - valuesOffset;

                var elemType = colCtor.GetElementType(argType); // Use for collection
                var valueType = elemType.UnwrapIfNullable(); // Use for parser
                var colInit = colCtor.Create(elemType, valuesUsed);
                var parser = _parserProvider.GetParser(valueType, arg.ValueParser);

                for (int i = 0; i < valuesUsed; i++)
                {
                    string stringValue = stringValues[valuesOffset + i];
                    var value = parser.Parse(stringValue, valueType, _formatProvider);
                    colInit.SetValue(i, value);
                }

                return colInit.Collection;
            }
            else
            {
                string stringValue = null;
                if (useLastValue)
                {
                    stringValue = stringValues[stringValues.Length - 1];
                    valuesUsed = stringValues.Length - valuesOffset;
                }
                else
                {
                    stringValue = stringValues[valuesOffset];
                    valuesUsed = 1;
                }

                if (argType == StringType)
                {
                    return stringValue;
                }

                var valueType = argType.UnwrapIfNullable();
                var parser = _parserProvider.GetParser(valueType, arg.ValueParser);
                var value = parser.Parse(stringValue, valueType, _formatProvider);
                return value;
            }
        }

        private object GetEmptyValue(Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        private Array CopyArray(Array source, int offset, int length)
        {
            var elementType = _arrayCtor.GetElementType(source.GetType());
            var copy = (Array)_arrayCtor.Create(elementType, length).Collection;
            Array.Copy(source, offset, copy, 0, length);
            return copy;
        }
    }
}
