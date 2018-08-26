using System;
using System.Collections.Generic;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class MultiValueParser : IMultiValueParser
    {
        private readonly ICollectionConstructorProvider _collectionCtorProvider;
        private readonly IValueParserProvider _valueParserProvider;

        public MultiValueParser(
            ICollectionConstructorProvider collectionCtorProvider,
            IValueParserProvider valueParserProvider)
        {
            _collectionCtorProvider = collectionCtorProvider ?? throw new ArgumentNullException(nameof(collectionCtorProvider));
            _valueParserProvider = valueParserProvider ?? throw new ArgumentNullException(nameof(valueParserProvider));
        }

        public MultiValueParseResult Parse(
            Type valueType,
            IReadOnlyList<string> stringValues,
            SingleValuePolicy singleValuePolicy = SingleValuePolicy.UseFirstValue,
            Type valueParserType = null,
            IFormatProvider formatProvider = null)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException(nameof(valueType));
            }
            if (stringValues == null)
            {
                throw new ArgumentNullException(nameof(stringValues));
            }
            if (stringValues.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(stringValues));
            }

            bool isCollection = _collectionCtorProvider.TryGetConstructor(valueType, out ICollectionConstructor colCtor);

            if (isCollection)
            {
                var elemType = colCtor.GetElementType(valueType);
                valueType = elemType.UnwrapIfNullable();
                var valueParser = _valueParserProvider.GetParser(valueType, valueParserType);

                int valuesCount = stringValues.Count;
                object[] values = new object[valuesCount];

                for (int i = 0; i < valuesCount; i++)
                {
                    values[i] = valueParser.Parse(stringValues[i], valueType, formatProvider);
                }

                var collection = colCtor.Construct(elemType, values);
                return new MultiValueParseResult(collection, valuesCount);
            }
            else
            {
                valueType = valueType.UnwrapIfNullable();
                var valueParser = _valueParserProvider.GetParser(valueType, valueParserType);

                string stringValue = GetSingleValue(stringValues, singleValuePolicy, out var valuesUsed);

                var value = valueParser.Parse(stringValue, valueType, formatProvider);
                return new MultiValueParseResult(value, valuesUsed);
            }
        }

        private static string GetSingleValue(
            IReadOnlyList<string> stringValues,
            SingleValuePolicy singleValuePolicy,
            out int valuesUsed)
        {
            switch (singleValuePolicy)
            {
                case SingleValuePolicy.UseFirstValue:
                    valuesUsed = 1;
                    return stringValues[0];
                case SingleValuePolicy.UseLastValue:
                    valuesUsed = stringValues.Count;
                    return stringValues[stringValues.Count - 1];
                default:
                    throw new NotSupportedException($"Unknown single value policy `{singleValuePolicy}`.");
            }
        }
    }
}
