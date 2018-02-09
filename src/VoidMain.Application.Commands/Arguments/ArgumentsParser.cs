using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface.Parser;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParser : IArgumentsParser
    {
        private readonly Type BooleanType = typeof(bool);
        private readonly object TrueValue = true;

        private readonly ICollectionConstructorProvider _colCtorProvider;
        private readonly IValueParserProvider _parserProvider;
        private readonly IFormatProvider _formatProvider;
        private readonly IEqualityComparer<string> _identifierComparer;

        public ArgumentsParser(
            ICollectionConstructorProvider colCtorProvider,
            IValueParserProvider parserProvider,
            ArgumentsParserOptions options = null,
            CommandLineSyntaxOptions syntaxOptions = null)
        {
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
            _parserProvider = parserProvider ?? throw new ArgumentNullException(nameof(parserProvider));
            _formatProvider = options?.FormatProvider ?? CultureInfo.CurrentCulture;
            _identifierComparer = syntaxOptions?.IdentifierComparer
                ?? CommandLineSyntaxOptions.DefaultIdentifierComparer;
        }

        public object[] Parse(IReadOnlyList<ArgumentModel> argsModel,
            KeyValuePair<string, string>[] options, string[] operands, IServiceProvider services)
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
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var values = new object[argsModel.Count];
            int operandsOffset = 0;

            for (int i = 0; i < argsModel.Count; i++)
            {
                var arg = argsModel[i];

                switch (arg.Kind)
                {
                    case ArgumentKind.Service:
                        values[i] = GetService(arg, services);
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

        private string[] GetOptionValues(KeyValuePair<string, string>[] options, ArgumentModel arg)
        {
            var buffer = new List<string>();

            foreach (var option in options)
            {
                if (IsNameOrAliasEquals(option.Key, arg))
                {
                    buffer.Add(option.Value);
                }
            }

            return buffer.ToArray();
        }

        private bool IsNameOrAliasEquals(string optionName, ArgumentModel arg)
        {
            return _identifierComparer.Equals(optionName, arg.Name)
                || _identifierComparer.Equals(optionName, arg.Alias);
        }

        private object GetService(ArgumentModel arg, IServiceProvider services)
        {
            var service = services.GetService(arg.Type);
            if (service != null)
            {
                return service;
            }

            if (arg.Optional)
            {
                return arg.Type.GetEmptyValue();
            }

            throw new ArgumentParseException(arg, $"Service '{arg.Type}' is not registered.");
        }

        private object ParseValueOrGetDefault(ArgumentModel arg, string[] stringValues,
            int valuesOffset, out int valuesUsed, bool useLastValue = false)
        {
            if (stringValues == null || valuesOffset == stringValues.Length)
            {
                valuesUsed = 0;
                return GetDefaultValue(arg, useLastValue);
            }

            return ParseValue(arg, stringValues, valuesOffset, out valuesUsed, useLastValue);
        }

        private object GetDefaultValue(ArgumentModel arg, bool useLastValue)
        {
            var defaultValue = arg.DefaultValue;
            if (defaultValue == null)
            {
                if (arg.Optional)
                {
                    return arg.Type.GetEmptyValue();
                }

                var valueType = arg.Type.UnwrapIfNullable();
                if (valueType == BooleanType)
                {
                    // Flag without a value is true by default.
                    return TrueValue;
                }

                throw new ArgumentParseException(arg, "Value is missing.");
                // TODO: Or prompt value.
                // Use double Enter to end filling collection.
            }

            switch (defaultValue)
            {
                case string stringValue:
                    return ParseValue(arg, new[] { stringValue }, 0, out var _, useLastValue);
                case string[] stringValues when stringValues.Length > 0:
                    return ParseValue(arg, stringValues, 0, out var _, useLastValue);
                default:
                    return TryToCast(arg, defaultValue);
            }
        }

        private object TryToCast(ArgumentModel arg, object value)
        {
            var argType = arg.Type;
            var valueType = value.GetType();

            bool isArgCollection = _colCtorProvider.TryGetCollectionConstructor(argType, out var argColCtor);
            bool isValueCollection = _colCtorProvider.TryGetCollectionConstructor(valueType, out var valueColCtor);

            if (isArgCollection && isValueCollection)
            {
                var argElemType = argColCtor.GetElementType(argType);
                var valueElemType = valueColCtor.GetElementType(valueType);
                if (!argElemType.GetTypeInfo().IsAssignableFrom(valueElemType))
                {
                    throw new ArgumentParseException(arg, "Element's types of the argument and default value do not match.");
                }

                return CopyCollection(valueColCtor, value, argColCtor, argType);
            }

            if (argType.GetTypeInfo().IsAssignableFrom(valueType))
            {
                return value;
            }

            throw new ArgumentParseException(arg, "Types of the argument and default value do not match.");
        }

        private object CopyCollection(
            ICollectionConstructor sourceCtor, object sourceCollection,
            ICollectionConstructor targetCtor, Type targetCollectionType)
        {
            var elementType = targetCtor.GetElementType(targetCollectionType);
            var source = sourceCtor.Wrap(sourceCollection);
            var target = targetCtor.Create(elementType, source.Count);

            for (int i = 0; i < source.Count; i++)
            {
                var value = source.GetValue(i);
                target.SetValue(i, value);
            }

            return target.Collection;
        }

        private object ParseValue(ArgumentModel arg, string[] stringValues,
            int valuesOffset, out int valuesUsed, bool useLastValue = false)
        {
            var argType = arg.Type;
            bool isCollection = _colCtorProvider.TryGetCollectionConstructor(
                argType, out ICollectionConstructor colCtor);

            if (isCollection)
            {
                valuesUsed = stringValues.Length - valuesOffset;

                var argElemType = colCtor.GetElementType(argType);
                var colAdapter = colCtor.Create(argElemType, valuesUsed);

                var valueType = argElemType.UnwrapIfNullable();
                var parser = _parserProvider.GetParser(valueType, arg.ValueParser);

                for (int i = 0; i < valuesUsed; i++)
                {
                    string stringValue = stringValues[valuesOffset + i];
                    var value = ParseValue(stringValue, valueType, parser);
                    colAdapter.SetValue(i, value);
                }

                return colAdapter.Collection;
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

                var parser = _parserProvider.GetParser(argType, arg.ValueParser);
                return ParseValue(stringValue, argType, parser);
            }
        }

        private object ParseValue(string stringValue, Type valueType, IValueParser parser)
        {
            if (stringValue == null)
            {
                if (valueType == BooleanType)
                {
                    return TrueValue;
                }
                stringValue = String.Empty;
            }

            return parser.Parse(stringValue, valueType, _formatProvider);
        }
    }
}
