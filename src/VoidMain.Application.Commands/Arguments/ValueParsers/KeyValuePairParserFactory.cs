using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class KeyValuePairParserFactory : IValueParserFactory
    {
        private readonly IValueParserProvider _valueParserProvider;

        public KeyValuePairParserFactory(IValueParserProvider valueParserProvider)
        {
            _valueParserProvider = valueParserProvider ?? throw new ArgumentNullException(nameof(valueParserProvider));
        }

        public IValueParser CreateInstance(Type valueType)
        {
            return new KeyValuePairParser(valueType, _valueParserProvider);
        }
    }
}
