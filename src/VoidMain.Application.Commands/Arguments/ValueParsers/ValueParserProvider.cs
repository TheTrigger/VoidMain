using System;
using System.Collections.Generic;
using System.Reflection;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ValueParserProvider : IValueParserProvider
    {
        private readonly IValueParser _defaultParser;
        private readonly IValueParser _enumParser;
        private readonly Dictionary<Type, IValueParser> _standardParsers;
        private readonly Dictionary<Type, IValueParser> _customParsersCache;

        public ValueParserProvider(ValueParserProviderOptions options = null)
        {
            _defaultParser = options?.DefaultParser ?? new ChangeTypeValueParser();
            _enumParser = options?.EnumParser ?? new EnumValueParser();
            _standardParsers = InitParsers(options?.ValueParsers);
            _customParsersCache = new Dictionary<Type, IValueParser>();
        }

        private Dictionary<Type, IValueParser> InitParsers(
            Dictionary<Type, IValueParser> customParsers)
        {
            var parsers = new Dictionary<Type, IValueParser>();

            // Default parsers.
            parsers[typeof(string)] = new StringValueParser();
            parsers[typeof(Guid)] = new GuidValueParser();
            parsers[typeof(IntPtr)] = new IntPtrValueParser();
            parsers[typeof(TimeSpan)] = new TimeSpanValueParser();
            parsers[typeof(Uri)] = new UriValueParser();

            // Custom parsers.
            if (customParsers?.Count > 0)
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

        public IValueParser GetParser(Type valueType, Type parserType)
        {
            IValueParser parser = null;
            if (parserType == null)
            {
                if (valueType.GetTypeInfo().IsEnum)
                {
                    return _enumParser;
                }
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
