using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public interface IMultiValueParser
    {
        MultiValueParseResult Parse(
            Type valueType,
            IReadOnlyList<string> stringValues,
            SingleValuePolicy singleValuePolicy = SingleValuePolicy.UseFirstValue,
            Type valueParserType = null,
            IFormatProvider formatProvider = null);
    }
}
