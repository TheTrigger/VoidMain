using System;

namespace VoidMain.Application.Commands.Arguments
{
    public class ChangeTypeValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType)
        {
            return Convert.ChangeType(stringValue, valueType);
        }
    }
}
