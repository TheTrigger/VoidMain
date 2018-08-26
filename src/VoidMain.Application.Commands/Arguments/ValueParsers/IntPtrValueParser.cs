using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class IntPtrValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider = null)
        {
            long value = Int64.Parse(stringValue, formatProvider);
            return new IntPtr(value);
        }
    }
}
