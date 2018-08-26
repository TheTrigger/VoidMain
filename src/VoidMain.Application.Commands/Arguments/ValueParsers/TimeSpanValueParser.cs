using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class TimeSpanValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider = null)
        {
            return TimeSpan.Parse(stringValue, formatProvider);
        }
    }
}
