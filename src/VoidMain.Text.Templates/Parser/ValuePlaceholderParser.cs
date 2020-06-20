using System;

namespace VoidMain.Text.Templates.Parser
{
    public class ValuePlaceholderParser<TValueKey, TValueKeyParser> : IPlaceholderParser<ValuePlaceholder<TValueKey>>
        where TValueKeyParser : IValueKeyParser<TValueKey>
    {
        private readonly TValueKeyParser _valueKeyParser;
        private readonly IFormatProvider _formatProvider;

        public ValuePlaceholderParser(
            TValueKeyParser valueKeyParser,
            IFormatProvider formatProvider = null)
        {
            _valueKeyParser = valueKeyParser ?? throw new ArgumentNullException(nameof(valueKeyParser));
            _formatProvider = formatProvider;
        }

        public int Parse<TPlaceholderConstraint>(string template, int position,
            TPlaceholderConstraint placeholderConstraint, out ValuePlaceholder<TValueKey> placeholder)
            where TPlaceholderConstraint : struct, IPlaceholderConstraint
        {
            int consumed = ParserKey(template, position, placeholderConstraint, out var key);
            consumed += ParseAlignment(template, position + consumed, placeholderConstraint, out int alignment);
            consumed += ParseFormat(template, position + consumed, placeholderConstraint, out var format);

            placeholder = new ValuePlaceholder<TValueKey>(key, alignment, format);
            return consumed;
        }

        protected int ParserKey<TPlaceholderConstraint>(
            string template, int position, TPlaceholderConstraint placeholderConstraint, out TValueKey key)
            where TPlaceholderConstraint : struct, IPlaceholderConstraint
        {
            int start = position;

            while (position < template.Length)
            {
                if (placeholderConstraint.IsEndOfPlaceholder(template, position))
                {
                    break;
                }

                char c = template[position];
                if (c == ',' || c == ':')
                {
                    break;
                }

                position++;
            }

            if (position == start)
            {
                throw new FormatException(); // TODO: Missing placeholder key
            }

            int consumed = position - start;
            var rawKey = template.AsSpan(start, consumed);
            key = _valueKeyParser.Parse(rawKey, _formatProvider);

            return consumed;
        }

        protected int ParseAlignment<TPlaceholderConstraint>(
            string template, int position, TPlaceholderConstraint placeholderConstraint, out int alignment)
            where TPlaceholderConstraint : struct, IPlaceholderConstraint
        {
            if (position >= template.Length || template[position] != ',')
            {
                alignment = default;
                return 0;
            }

            position++;
            int start = position;

            while (position < template.Length)
            {
                if (placeholderConstraint.IsEndOfPlaceholder(template, position))
                {
                    break;
                }

                char c = template[position];
                if (c == ':')
                {
                    break;
                }

                position++;
            }

            if (position == start)
            {
                throw new FormatException(); // TODO: Missing placeholder alignment
            }

            int consumed = position - start;
            var rawAlignment = template.AsSpan(start, consumed);
            alignment = Int32.Parse(rawAlignment, provider: _formatProvider);

            consumed++; // Taking into account the consumed comma
            return consumed;
        }

        protected int ParseFormat<TPlaceholderConstraint>(
            string template, int position, TPlaceholderConstraint placeholderConstraint, out ReadOnlyMemory<char> format)
            where TPlaceholderConstraint : struct, IPlaceholderConstraint
        {
            if (position >= template.Length || template[position] != ':')
            {
                format = default;
                return 0;
            }

            position++;
            int start = position;

            while (position < template.Length)
            {
                if (placeholderConstraint.IsEndOfPlaceholder(template, position))
                {
                    break;
                }

                position++;
            }

            if (position == start)
            {
                throw new FormatException(); // TODO: Missing placeholder format
            }

            int consumed = position - start;
            format = template.AsMemory(start, consumed);

            consumed++; // Taking into account the consumed colon
            return consumed;
        }
    }
}
