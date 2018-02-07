using System;

namespace VoidMain.Application.Commands.Arguments
{
    public class StringValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType, IFormatProvider provider)
        {
            return stringValue;
        }
    }
}
