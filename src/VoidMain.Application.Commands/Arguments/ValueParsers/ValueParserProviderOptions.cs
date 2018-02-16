using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ValueParserProviderOptions
    {
        public IValueParser DefaultParser { get; set; }
        public Dictionary<Type, IValueParser> ValueParsers { get; set; } = new Dictionary<Type, IValueParser>();
    }
}
