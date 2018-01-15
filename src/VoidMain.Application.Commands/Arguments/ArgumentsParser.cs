using System;
using System.Collections.Generic;
using System.Reflection;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParser : IArgumentsParser
    {
        private readonly Type StringType = typeof(string);

        private readonly IServiceProvider _services;
        private readonly ICollectionConstructor _arrayCtor;
        private readonly Dictionary<Type, ICollectionConstructor> _colCtors;

        private readonly IValueParser _defaultParser;
        private readonly Dictionary<Type, IValueParser> _standardParsers;
        private readonly Dictionary<Type, IValueParser> _customParsersCache;

        public ArgumentsParser(IServiceProvider services, ArgumentsParserOptions options = null)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _arrayCtor = options?.ArrayConstructor ?? new ArrayConstructor();
            _colCtors = InitCollectionConstructors(options?.CollectionConstructors);
            _defaultParser = options?.DefaultParser ?? new ChangeTypeValueParser();
            _standardParsers = InitStandardParsers(options?.ValueParsers);
            _customParsersCache = new Dictionary<Type, IValueParser>();
        }

        private Dictionary<Type, ICollectionConstructor> InitCollectionConstructors(
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
            if (customCtors != null && customCtors.Count > 0)
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

        private Dictionary<Type, IValueParser> InitStandardParsers(
            Dictionary<Type, IValueParser> customParsers)
        {
            var parsers = new Dictionary<Type, IValueParser>();

            // Custom parsers.
            if (customParsers != null && customParsers.Count > 0)
            {
                foreach (var custom in customParsers)
                {
                    if (custom.Value == null)
                    {
                        parsers.Remove(custom.Key);
                    }
                    else
                    {
                        parsers[custom.Key] = custom.Value;
                    }
                }
            }

            return parsers;
        }

        public object[] Parse(
            IReadOnlyList<ArgumentModel> argsModel,
            Dictionary<string, string[]> options, string[] operands)
        {
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

                var argType = arg.Type;
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

            if (TryGetCollectionConstructor(argType, out ICollectionConstructor colCtor))
            {
                valuesUsed = stringValues.Length - valuesOffset;

                var valueType = colCtor.GetElementType(argType);
                var colInit = colCtor.Create(valueType, valuesUsed);
                var parser = GetParser(valueType, arg.ValueParser);

                for (int i = 0; i < valuesUsed; i++)
                {
                    string stringValue = stringValues[valuesOffset + i];
                    var value = parser.Parse(stringValue, valueType);
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

                var parser = GetParser(argType, arg.ValueParser);
                var value = parser.Parse(stringValue, argType);
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

        private bool TryGetCollectionConstructor(
            Type collectionType, out ICollectionConstructor colCtor)
        {
            if (_colCtors.TryGetValue(collectionType, out colCtor))
            {
                return true;
            }

            if (collectionType.IsArray)
            {
                colCtor = _arrayCtor;
                return true;
            }

            if (!collectionType.GetTypeInfo().IsGenericType)
            {
                colCtor = null;
                return false;
            }

            var genericDefinition = collectionType.GetGenericTypeDefinition();
            return _colCtors.TryGetValue(genericDefinition, out colCtor);
        }

        private IValueParser GetParser(Type valueType, Type parserType)
        {
            IValueParser parser = null;
            if (parserType == null)
            {
                if (!_standardParsers.TryGetValue(valueType, out parser))
                {
                    parser = _defaultParser;
                }
            }
            else if (!_customParsersCache.TryGetValue(parserType, out parser))
            {
                parser = (IValueParser)Activator.CreateInstance(parserType);
                _customParsersCache.Add(parserType, parser);
            }
            return parser;
        }
    }
}
