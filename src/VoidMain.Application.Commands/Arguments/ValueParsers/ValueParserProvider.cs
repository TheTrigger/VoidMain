using System;
using System.Reflection;
using VoidMain.Application.Commands.Internal;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ValueParserProvider : IValueParserProvider
    {
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
                return GetInstance(parserType, _services);
            }

            if (valueType.GetTypeInfo().IsEnum)
            {
                return GetInstance(_options.EnumParser, _services);
            }

            if (_options.ValueParsers.TryGetValue(valueType, out var config))
            {
                return GetInstance(config, _services);
            }

            if (valueType.GetTypeInfo().IsGenericType)
            {
                var definition = valueType.GetGenericTypeDefinition();
                if (_options.ValueParsers.TryGetValue(definition, out config))
                {
                    return GetInstance(config, _services);
                }
            }

            return GetInstance(_options.DefaultParser, _services);
        }

        private IValueParser GetInstance(Type parserType, IServiceProvider services)
        {
            return (IValueParser)_typeActivator.CreateInstance(parserType, services);
        }

        private IValueParser GetInstance(TypeOrInstance<IValueParser> config, IServiceProvider services)
        {
            return config.Instance ?? GetInstance(config.Type, services);
        }
    }
}
