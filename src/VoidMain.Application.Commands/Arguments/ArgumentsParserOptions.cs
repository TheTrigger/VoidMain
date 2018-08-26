using System;
using System.Globalization;
using VoidMain.Application.Commands.Arguments.ValueParsers;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParserOptions
    {
        public IFormatProvider FormatProvider { get; set; }
        public SingleValuePolicy OptionsSingleValuePolicy { get; set; }

        public ArgumentsParserOptions(bool defaults = true)
        {
            if (defaults)
            {
                FormatProvider = CultureInfo.CurrentCulture;
                OptionsSingleValuePolicy = SingleValuePolicy.UseLastValue;
            }
        }

        public void Validate()
        {
            if (FormatProvider == null)
            {
                throw new ArgumentNullException(nameof(FormatProvider));
            }
        }
    }
}
