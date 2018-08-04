using System;
using System.Reflection;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ValueParserProvider : IValueParserProvider
    {
        private static readonly TypeInfo FactoryType = typeof(IValueParserFactory).GetTypeInfo();
        private static readonly TypeInfo ParserType = typeof(IValueParser).GetTypeInfo();
        private const string NotSupportedTypeError = "Type {0} must implemeent "
            + nameof(IValueParser) + " or " + nameof(IValueParserFactory) + " interface.";

        private readonly ITypeActivator _typeActivator;
        private readonly IServiceProvider _services;
        private readonly ValueParserProviderOptions _options;

        public ValueParserProvider(
            ITypeActivator typeActivator,
            IServiceProvider services,
            ValueParserProviderOptions options = null)
        {
            _typeActivator = typeActivator ?? throw new ArgumentNullException(nameof(typeActivator));
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _options = options ?? new ValueParserProviderOptions();
            _options.Validate();
        }

        public IValueParser GetParser(Type valueType, Type parserType)
        {
            if (parserType != null)
            {
                return CreateInstance<IValueParser>(parserType, _services);
            }

            if (valueType.GetTypeInfo().IsEnum)
            {
                return GetInstance(_options.EnumParser, valueType, _services);
            }

            if (_options.ValueParsers.TryGetValue(valueType, out var config))
            {
                return GetInstance(config, valueType, _services);
            }

            if (valueType.GetTypeInfo().IsGenericType)
            {
                var definition = valueType.GetGenericTypeDefinition();
                if (_options.ValueParsers.TryGetValue(definition, out config))
                {
                    return GetInstance(config, valueType, _services);
                }
            }

            return GetInstance(_options.DefaultParser, valueType, _services);
        }

        private T CreateInstance<T>(Type type, IServiceProvider services)
        {
            return (T)_typeActivator.CreateInstance(type, services);
        }

        private IValueParser GetInstance(TypeOrInstance<IValueParserMetadata> config, Type valueType, IServiceProvider services)
        {
            if (config.Type == null)
            {
                if (config.Instance is IValueParserFactory factory)
                {
                    return factory.CreateInstance(valueType);
                }
                else if (config.Instance is IValueParser parser)
                {
                    return parser;
                }
            }
            else
            {
                if (FactoryType.IsAssignableFrom(config.Type))
                {
                    var factory = CreateInstance<IValueParserFactory>(config.Type, services);
                    return factory.CreateInstance(valueType);
                }
                else if (ParserType.IsAssignableFrom(config.Type))
                {
                    return CreateInstance<IValueParser>(config.Type, services);
                }
            }

            throw new NotSupportedException(String.Format(NotSupportedTypeError, config.GetImplType().Name));
        }
    }
}
