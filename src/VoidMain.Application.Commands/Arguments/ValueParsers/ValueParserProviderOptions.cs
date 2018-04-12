using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ValueParserProviderOptions
    {
        public IValueParser DefaultParser { get; set; }
        public IValueParser EnumParser { get; set; }
        public Dictionary<Type, IValueParser> ValueParsers { get; set; }

        public ValueParserProviderOptions(bool defaults = true)
        {
            if (defaults)
            {
                DefaultParser = new ChangeTypeValueParser();
                EnumParser = new EnumValueParser();
                ValueParsers = new Dictionary<Type, IValueParser>
                {
                    [typeof(String)] = new StringValueParser(),
                    [typeof(Guid)] = new GuidValueParser(),
                    [typeof(IntPtr)] = new IntPtrValueParser(),
                    [typeof(TimeSpan)] = new TimeSpanValueParser(),
                    [typeof(Uri)] = new UriValueParser()
                };
            }
        }

        public void Validate()
        {
            if (DefaultParser == null)
            {
                throw new ArgumentNullException(nameof(DefaultParser));
            }
            if (EnumParser == null)
            {
                throw new ArgumentNullException(nameof(EnumParser));
            }
            if (ValueParsers == null)
            {
                throw new ArgumentNullException(nameof(ValueParsers));
            }
        }
    }
}
