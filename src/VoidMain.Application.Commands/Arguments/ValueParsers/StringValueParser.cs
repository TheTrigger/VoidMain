using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class StringValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider = null)
        {
            return stringValue;
        }
    }
}
