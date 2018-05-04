using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public interface IValueParserProvider
    {
        IValueParser GetParser(Type valueType, Type parserType);
    }
}
