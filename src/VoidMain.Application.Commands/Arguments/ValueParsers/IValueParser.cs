using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public interface IValueParser : IValueParserMetadata
    {
        object Parse(string stringValue, Type valueType, IFormatProvider formatProvider = null);
    }
}
