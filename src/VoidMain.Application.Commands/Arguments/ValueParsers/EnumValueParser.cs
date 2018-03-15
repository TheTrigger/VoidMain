using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class EnumValueParser : IValueParser
    {
        private readonly bool _ignoreCase;

        public EnumValueParser(bool ignoreCase = true)
        {
            _ignoreCase = ignoreCase;
        }

        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider)
        {
            return Enum.Parse(valueType, stringValue, _ignoreCase);
        }
    }
}
