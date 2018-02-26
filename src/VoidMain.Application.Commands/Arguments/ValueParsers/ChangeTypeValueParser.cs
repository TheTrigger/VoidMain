﻿using System;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ChangeTypeValueParser : IValueParser
    {
        public object Parse(string stringValue, Type valueType, IFormatProvider formatProvider)
        {
            return Convert.ChangeType(stringValue, valueType, formatProvider);
        }
    }
}