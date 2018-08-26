using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class BooleanValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider = null)
        {
            if (String.IsNullOrEmpty(stringValue))
            {
                return true;
            }

            return Boolean.Parse(stringValue);
        }
    }
}
