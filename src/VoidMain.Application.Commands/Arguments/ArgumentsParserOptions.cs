using System;
using System.Globalization;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentsParserOptions
    {
        public IFormatProvider FormatProvider { get; set; }
        public MultiValueStrategy MultiValueStrategy { get; set; }

        public ArgumentsParserOptions(bool defaults = true)
        {
            if (defaults)
            {
                FormatProvider = CultureInfo.CurrentCulture;
                MultiValueStrategy = MultiValueStrategy.UseLastValue;
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
