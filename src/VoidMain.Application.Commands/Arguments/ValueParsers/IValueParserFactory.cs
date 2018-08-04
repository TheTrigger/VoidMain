using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public interface IValueParserFactory : IValueParserMetadata
    {
        IValueParser CreateInstance(Type valueType);
    }
}
