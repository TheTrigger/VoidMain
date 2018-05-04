using System;
using System.Collections.Generic;
using System.Reflection;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Arguments.ValueParsers;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParser : IArgumentsParser
    {
        private const string CastErrorMessagTemplate = "Type `{0}` of the default value " +
            "is not assignable to the type `{1}` of the argument.";

        private const int OptionValuesOffset = 0;
        private const MultiValueStrategy OperandMultiValueStrategy = MultiValueStrategy.UseFirstValue;
        private readonly Type BooleanType = typeof(bool);
        private readonly object TrueValue = true;

        private readonly ICollectionConstructorProvider _colCtorProvider;
        private readonly IValueParserProvider _parserProvider;
        private readonly ArgumentsParserOptions _options;
        private readonly CommandLineOptions _cliOptions;

        public ArgumentsParser(
            ICollectionConstructorProvider colCtorProvider,
            IValueParserProvider parserProvider,
            ArgumentsParserOptions options = null,
            CommandLineOptions cliOptions = null)
        {
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
            _parserProvider = parserProvider ?? throw new ArgumentNullException(nameof(parserProvider));
            _options = options ?? new ArgumentsParserOptions();
            _options.Validate();
            _cliOptions = cliOptions ?? new CommandLineOptions();
            _cliOptions.Validate();
        }

        private static void Validate(IReadOnlyList<ArgumentModel> argsModel,
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
        }

        public object[] Parse(IReadOnlyList<ArgumentModel> argsModel,
            KeyValuePair<string, string>[] options, string[] operands, IServiceProvider services)
        {
            Validate(argsModel, options, operands, services);

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
                        values[i] = ParseValueOrGetDefault(
                            arg, optionValues, OptionValuesOffset, services, _options.MultiValueStrategy, out int _);
                        break;
                    case ArgumentKind.Operand:
                        values[i] = ParseValueOrGetDefault(
                            arg, operands, operandsOffset, services, OperandMultiValueStrategy, out int operandsUsed);
                        operandsOffset += operandsUsed;
                        break;
                    default:
                        throw new ArgumentParseException(arg, $"Unknown argument kind `{arg.Kind}`.");
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
            return _cliOptions.IdentifierComparer.Equals(optionName, arg.Name)
                || _cliOptions.IdentifierComparer.Equals(optionName, arg.Alias);
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

            throw new ArgumentParseException(arg, $"Service `{arg.Type.Name}` is not registered.");
        }

        private object ParseValueOrGetDefault(
            ArgumentModel arg,
            string[] stringValues, int valuesOffset,
            IServiceProvider services,
            MultiValueStrategy multiValueStrategy,
            out int valuesUsed)
        {
            if (stringValues == null || valuesOffset == stringValues.Length)
            {
                valuesUsed = 0;
                return GetDefaultValue(arg, services, multiValueStrategy);
            }

            return ParseValue(arg, stringValues, valuesOffset, multiValueStrategy, out valuesUsed);
        }

        private object GetDefaultValue(ArgumentModel arg, IServiceProvider services, MultiValueStrategy multiValueStrategy)
        {
            var defaultValue = arg.DefaultValue;
            if (defaultValue == null)
            {
                if (arg.Optional)
                {
                    return arg.Type.GetEmptyValue();
                }

                throw new ArgumentParseException(arg, "Value is missing.");
                // TODO: Or prompt value.
                // Use double Enter to end filling collection.
            }

            switch (defaultValue)
            {
                case string stringValue:
                    return ParseValue(arg, new[] { stringValue }, 0, multiValueStrategy, out var _);
                case string[] stringValues when stringValues.Length > 0:
                    return ParseValue(arg, stringValues, 0, multiValueStrategy, out var _);
                default:
                    return CastDefaultValue(arg, defaultValue);
            }
        }

        private object CastDefaultValue(ArgumentModel arg, object value)
        {
            var argType = arg.Type;
            var valueType = value.GetType();

            bool isArgCollection = _colCtorProvider.TryGetConstructor(argType, out var argColCtor);
            bool isValueCollection = _colCtorProvider.TryGetConstructor(valueType, out var valueColCtor);

            if (isArgCollection && isValueCollection)
            {
                var argElemType = argColCtor.GetElementType(argType);
                var valueElemType = valueColCtor.GetElementType(valueType);
                if (!argElemType.GetTypeInfo().IsAssignableFrom(valueElemType))
                {
                    throw new ArgumentParseException(arg,
                        String.Format(CastErrorMessagTemplate, valueElemType.Name, argElemType.Name));
                }

                return CopyCollection(valueColCtor, value, argColCtor, argType);
            }

            if (argType.GetTypeInfo().IsAssignableFrom(valueType))
            {
                return value;
            }

            throw new ArgumentParseException(arg,
                        String.Format(CastErrorMessagTemplate, valueType.Name, argType.Name));
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

        private object ParseValue(
            ArgumentModel arg,
            string[] stringValues, int valuesOffset,
            MultiValueStrategy multiValueStrategy,
            out int valuesUsed)
        {
            var argType = arg.Type;
            bool isCollection = _colCtorProvider.TryGetConstructor(argType, out ICollectionConstructor colCtor);

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
                string stringValue = GetStringValue(
                    stringValues, valuesOffset, multiValueStrategy, out valuesUsed);
                var valueType = argType.UnwrapIfNullable();
                var parser = _parserProvider.GetParser(valueType, arg.ValueParser);
                return ParseValue(stringValue, valueType, parser);
            }
        }

        private static string GetStringValue(
            string[] stringValues, int valuesOffset,
            MultiValueStrategy multiValueStrategy,
            out int valuesUsed)
        {
            switch (multiValueStrategy)
            {
                case MultiValueStrategy.UseFirstValue:
                    valuesUsed = 1;
                    return stringValues[valuesOffset];
                case MultiValueStrategy.UseLastValue:
                    valuesUsed = stringValues.Length - valuesOffset;
                    return stringValues[stringValues.Length - 1];
                default:
                    throw new NotSupportedException($"Unknown multivalue strategy `{multiValueStrategy}`.");
            }
        }

        private object ParseValue(string stringValue, Type valueType, IValueParser parser)
        {
            if (stringValue == null)
            {
                if (valueType == BooleanType)
                {
                    // Flag without a value is true by default.
                    return TrueValue;
                }
                stringValue = String.Empty;
            }

            return parser.Parse(stringValue, valueType, _options.FormatProvider);
        }
    }
}
