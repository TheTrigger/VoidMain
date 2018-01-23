using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments
{
    public class ValueParserProviderOptions
    {
        public IValueParser DefaultParser { get; set; }
        public Dictionary<Type, IValueParser> ValueParsers { get; set; }

        public ValueParserProviderOptions()
        {
            ValueParsers = new Dictionary<Type, IValueParser>();
        }
    }
}
