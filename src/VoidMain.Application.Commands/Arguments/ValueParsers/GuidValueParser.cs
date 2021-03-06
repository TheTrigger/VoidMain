﻿using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class GuidValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider = null)
        {
            return Guid.Parse(stringValue);
        }
    }
}
