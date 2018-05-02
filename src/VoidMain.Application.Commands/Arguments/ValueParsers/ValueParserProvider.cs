using System;
using System.Reflection;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ValueParserProvider : IValueParserProvider
    {
        private readonly ITypeActivator _typeActivator;
        private readonly ValueParserProviderOptions _options;

        public ValueParserProvider(
            ITypeActivator typeActivator,
            ValueParserProviderOptions options = null)
        {
            _typeActivator = typeActivator ?? throw new ArgumentNullException(nameof(typeActivator));
            _options = options ?? new ValueParserProviderOptions();
            _options.Validate();
        }

        public IValueParser GetParser(Type valueType, Type parserType, IServiceProvider services)
        {
            if (parserType != null)
            {
                return GetInstance(parserType, services);
            }

            if (valueType.GetTypeInfo().IsEnum)
            {
                return GetInstance(_options.EnumParser, services);
            }

            if (_options.ValueParsers.TryGetValue(valueType, out var parser))
            {
                return GetInstance(parser, services);
            }

            return GetInstance(_options.DefaultParser, services);
        }

        private IValueParser GetInstance(Type parserType, IServiceProvider services)
        {
            return (IValueParser)_typeActivator.CreateInstance(parserType, services);
        }

        private IValueParser GetInstance(TypeOrInstance<IValueParser> parser, IServiceProvider services)
        {
            return parser.Instance == null
                ? GetInstance(parser.Type, services)
                : parser.Instance;
        }
    }
}
