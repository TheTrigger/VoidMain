using System;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParserOptions
    {
        public IFormatProvider FormatProvider { get; set; }
        public MultiValueStrategy MultiValueStrategy { get; set; } = MultiValueStrategy.UseLastValue;
    }
}
