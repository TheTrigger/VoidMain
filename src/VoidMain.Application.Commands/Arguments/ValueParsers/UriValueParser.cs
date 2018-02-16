using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class UriValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider)
        {
            return new Uri(stringValue);
        }
    }
}
