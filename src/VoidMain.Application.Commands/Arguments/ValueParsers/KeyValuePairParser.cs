using System;
using System.Collections.Generic;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class KeyValuePairParser : IValueParser
    {
        private static readonly char[] Separators = { '=', ':' };
        private static readonly Type KeyValyeDefinition = typeof(KeyValuePair<,>);

        private readonly IValueParserProvider _valueParserProvider;

        public KeyValuePairParser(IValueParserProvider valueParserProvider)
        {
            _valueParserProvider = valueParserProvider ?? throw new ArgumentNullException(nameof(valueParserProvider));
        }

        public object Parse(string stringValue, Type keyValueType, IFormatProvider formatProvider)
        {
            if (!keyValueType.IsParameterizedGeneric())
            {
                throw new NotSupportedException();
            }

            var definition = keyValueType.GetGenericTypeDefinition();
            if (definition != KeyValyeDefinition)
            {
                throw new NotSupportedException();
            }

            int separatorIndex = stringValue.IndexOfAny(Separators);
            if (separatorIndex < 1)
            {
                throw new FormatException();
            }

            var keyType = keyValueType.GenericTypeArguments[0];
            string keyString = stringValue.Substring(0, separatorIndex);
            var keyParser = _valueParserProvider.GetParser(keyType, parserType: null);
            object key = keyParser.Parse(keyString, keyType, formatProvider);

            var valueType = keyValueType.GenericTypeArguments[1];
            string valueString = stringValue.Substring(separatorIndex + 1);
            var valueParser = _valueParserProvider.GetParser(valueType, parserType: null);
            object value = valueParser.Parse(valueString, valueType, formatProvider);

            var keyValue = Activator.CreateInstance(keyValueType, key, value);
            return keyValue;
        }
    }
}
