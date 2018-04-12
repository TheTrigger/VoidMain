using System;
using System.Collections.Generic;
using System.Reflection;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ValueParserProvider : IValueParserProvider
    {
        private readonly ValueParserProviderOptions _options;
        private readonly Dictionary<Type, IValueParser> _customParsersCache;

        public ValueParserProvider(ValueParserProviderOptions options = null)
        {
            _options = options ?? new ValueParserProviderOptions();
            _options.Validate();
            _customParsersCache = new Dictionary<Type, IValueParser>();
        }

        public IValueParser GetParser(Type valueType, Type parserType)
        {
            IValueParser parser = null;
            if (parserType == null)
            {
                if (valueType.GetTypeInfo().IsEnum)
                {
                    return _options.EnumParser;
                }
                if (!_options.ValueParsers.TryGetValue(valueType, out parser))
                {
                    parser = _options.DefaultParser;
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
