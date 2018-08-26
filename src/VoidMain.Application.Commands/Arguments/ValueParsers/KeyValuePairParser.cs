using System;
using System.Collections.Generic;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class KeyValuePairParser : IValueParser
    {
        private static readonly char[] Separators = { '=', ':' };
        private static readonly Type KeyValyeDefinition = typeof(KeyValuePair<,>);

        private readonly Type _keyValueType;
        private readonly Type _keyType;
        private readonly Type _valueType;
        private readonly IValueParser _keyParser;
        private readonly IValueParser _valueParser;

        public KeyValuePairParser(Type keyValueType, IValueParserProvider valueParserProvider)
        {
            if (valueParserProvider == null)
            {
                throw new ArgumentNullException(nameof(valueParserProvider));
            }
            _keyValueType = keyValueType ?? throw new ArgumentNullException(nameof(keyValueType));

            if (!IsParameterizedKeyValuePair(keyValueType))
            {
                throw new ArgumentException(
                    "Value type must be a parameterized KeyValuePair<TKey, TValye> type.",
                    nameof(keyValueType));
            }

            _keyType = keyValueType.GenericTypeArguments[0];
            _valueType = keyValueType.GenericTypeArguments[1];

            _keyParser = valueParserProvider.GetParser(_keyType, parserType: null);
            _valueParser = valueParserProvider.GetParser(_valueType, parserType: null);
        }

        private bool IsParameterizedKeyValuePair(Type type)
        {
            return type.IsParameterizedGeneric()
                && type.GetGenericTypeDefinition() == KeyValyeDefinition;
        }

        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider = null)
        {
            if (valueType != _keyValueType)
            {
                throw new ArgumentException(
                    "Value type must be a " + _keyValueType.Name + " type.",
                    nameof(valueType));
            }

            int separatorIndex = stringValue.IndexOfAny(Separators);
            if (separatorIndex < 0)
            {
                throw new FormatException("Key-value separator was not found.");
            }

            string keyString = stringValue.Substring(0, separatorIndex);
            string valueString = stringValue.Substring(separatorIndex + 1);

            object key = _keyParser.Parse(keyString, _keyType, formatProvider);
            object value = _valueParser.Parse(valueString, _valueType, formatProvider);

            var keyValue = Activator.CreateInstance(_keyValueType, key, value);
            return keyValue;
        }
    }
}
