﻿using System;
using System.Collections.Generic;
using System.Reflection;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Arguments.ValueParsers;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineInterface;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParser : IArgumentsParser
    {
        private class ParseArguments
        {
            public string[] Values { get; set; }
            public int ValuesOffset { get; set; }
            public SingleValuePolicy SingleValuePolicy { get; set; }
            public IServiceProvider Services { get; set; }
        }

        private const string CastErrorMessagTemplate =
            "Type `{0}` of the default value is not assignable to the type `{1}` of the argument.";
        private readonly Type BooleanType = typeof(bool);
        private readonly object TrueValue = true;
        private static bool NotParseException(Exception ex) => !(ex is ArgumentParseException);

        private readonly ICollectionConstructorProvider _collectionCtorProvider;
        private readonly IMultiValueParser _multiValueParser;
        private readonly ArgumentsParserOptions _options;
        private readonly CommandLineOptions _cliOptions;

        public ArgumentsParser(
            ICollectionConstructorProvider collectionCtorProvider,
            IMultiValueParser multiValueParser,
            ArgumentsParserOptions options = null,
            CommandLineOptions cliOptions = null)
        {
            _collectionCtorProvider = collectionCtorProvider ?? throw new ArgumentNullException(nameof(collectionCtorProvider));
            _multiValueParser = multiValueParser ?? throw new ArgumentNullException(nameof(multiValueParser));
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

        public object[] Parse(
            IReadOnlyList<ArgumentModel> argsModel,
            KeyValuePair<string, string>[] options,
            string[] operands,
            IServiceProvider services)
        {
            Validate(argsModel, options, operands, services);

            var values = new object[argsModel.Count];
            var parseArgs = new ParseArguments { Services = services };
            int operandsOffset = 0;

            for (int i = 0; i < argsModel.Count; i++)
            {
                var arg = argsModel[i];
                try
                {
                    switch (arg.Kind)
                    {
                        case ArgumentKind.Service:
                            parseArgs.Values = null;
                            parseArgs.ValuesOffset = 0;
                            parseArgs.SingleValuePolicy = _options.OptionsSingleValuePolicy;
                            values[i] = GetServiceOrDefault(arg, parseArgs);
                            break;
                        case ArgumentKind.Option:
                            parseArgs.Values = GetOptionValues(options, arg);
                            parseArgs.ValuesOffset = 0;
                            parseArgs.SingleValuePolicy = _options.OptionsSingleValuePolicy;
                            values[i] = ParseValueOrGetDefault(arg, parseArgs, out int _);
                            break;
                        case ArgumentKind.Operand:
                            parseArgs.Values = operands;
                            parseArgs.ValuesOffset = operandsOffset;
                            parseArgs.SingleValuePolicy = SingleValuePolicy.UseFirstValue;
                            values[i] = ParseValueOrGetDefault(arg, parseArgs, out int operandsUsed);
                            operandsOffset += operandsUsed;
                            break;
                        default:
                            throw new ArgumentParseException(arg, $"Unknown argument kind `{arg.Kind}`.");
                    }
                }
                catch (Exception ex) when (NotParseException(ex))
                {
                    throw new ArgumentParseException(arg, ex.Message, ex);
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

        private object GetServiceOrDefault(ArgumentModel arg, ParseArguments parseArgs)
        {
            var service = parseArgs.Services.GetService(arg.Type);
            if (service != null)
            {
                return service;
            }

            if (!TryGetDefaultValue(arg, parseArgs, out var value))
            {
                throw new ArgumentParseException(arg, $"Service `{arg.Type.Name}` is not registered.");
            }

            return value;
        }

        private object ParseValueOrGetDefault(ArgumentModel arg, ParseArguments parseArgs, out int valuesUsed)
        {
            if (parseArgs.Values == null || parseArgs.ValuesOffset == parseArgs.Values.Length)
            {
                if (!TryGetDefaultValue(arg, parseArgs, out var value))
                {
                    // TODO: Or prompt value.
                    // Use double Enter to end filling collection.
                    throw new ArgumentParseException(arg, "Value is missing.");
                }

                valuesUsed = 0;
                return value;
            }

            return ParseValue(arg, parseArgs, out valuesUsed);
        }

        private bool TryGetDefaultValue(ArgumentModel arg, ParseArguments parseArgs, out object defaultValue)
        {
            defaultValue = arg.DefaultValue;
            if (defaultValue == null)
            {
                if (arg.Optional)
                {
                    defaultValue = arg.Type.GetEmptyValue();
                    return true;
                }

                return false;
            }

            switch (defaultValue)
            {
                case string stringValue:
                    parseArgs.Values = new[] { stringValue };
                    parseArgs.ValuesOffset = 0;
                    defaultValue = ParseValue(arg, parseArgs, out var _);
                    break;
                case string[] stringValues when stringValues.Length > 0:
                    parseArgs.Values = stringValues;
                    parseArgs.ValuesOffset = 0;
                    defaultValue = ParseValue(arg, parseArgs, out var _);
                    break;
                default:
                    defaultValue = CastDefaultValue(arg, defaultValue);
                    break;
            }

            return true;
        }

        private object CastDefaultValue(ArgumentModel arg, object value)
        {
            var argType = arg.Type;
            var valueType = value.GetType();

            bool isArgCollection = _collectionCtorProvider.TryGetConstructor(argType, out var argColCtor);
            bool isValueCollection = _collectionCtorProvider.TryGetConstructor(valueType, out var valueColCtor);

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
            int count = sourceCtor.GetElementsCount(sourceCollection);
            object[] values = new object[count];
            sourceCtor.Deconstruct(sourceCollection, values);

            var targetElementType = targetCtor.GetElementType(targetCollectionType);
            var targetCollection = targetCtor.Construct(targetElementType, values);

            return targetCollection;
        }

        private object ParseValue(ArgumentModel arg, ParseArguments parseArgs, out int valuesUsed)
        {
            IReadOnlyList<string> stringValues = null;

            if(parseArgs.ValuesOffset == 0)
            {
                stringValues = parseArgs.Values;
            }
            else
            {
                int valuesCount = parseArgs.Values.Length - parseArgs.ValuesOffset;
                stringValues = new ArraySegment<string>(parseArgs.Values, parseArgs.ValuesOffset, valuesCount);
            }

            var result = _multiValueParser.Parse(
                arg.Type, stringValues, parseArgs.SingleValuePolicy,
                arg.ValueParser, _options.FormatProvider);

            valuesUsed = result.ValuesUsed;
            return result.Value;
        }
    }
}
