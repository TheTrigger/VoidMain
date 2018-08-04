using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Arguments.ValueParsers
{
    public class ValueParserProviderOptions
    {
        public TypeOrInstance<IValueParserMetadata> DefaultParser { get; set; }
        public TypeOrInstance<IValueParserMetadata> EnumParser { get; set; }
        public Dictionary<Type, TypeOrInstance<IValueParserMetadata>> ValueParsers { get; set; }

        public ValueParserProviderOptions(bool defaults = true)
        {
            if (defaults)
            {
                DefaultParser = typeof(ChangeTypeValueParser);
                EnumParser = typeof(EnumValueParser);
                ValueParsers = new Dictionary<Type, TypeOrInstance<IValueParserMetadata>>
                {
                    [typeof(String)] = typeof(StringValueParser),
                    [typeof(Guid)] = typeof(GuidValueParser),
                    [typeof(IntPtr)] = typeof(IntPtrValueParser),
                    [typeof(TimeSpan)] = typeof(TimeSpanValueParser),
                    [typeof(Uri)] = typeof(UriValueParser)
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
