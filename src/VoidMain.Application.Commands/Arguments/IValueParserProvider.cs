using System;

namespace VoidMain.Application.Commands.Arguments
{
    public interface IValueParserProvider
    {
        IValueParser GetParser(Type valueType, Type parserType);
    }
}
