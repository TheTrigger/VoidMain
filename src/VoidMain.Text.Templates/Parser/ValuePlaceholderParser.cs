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

        public int Parse<TParseRange>(
            string template, int position,
            TParseRange range, out ValuePlaceholder<TValueKey> placeholder)
            where TParseRange : struct, IParseRange
        {
            int consumed = ParserKey(template, position, range, out var key);
            consumed += ParseAlignment(template, position + consumed, range, out int alignment);
            consumed += ParseFormat(template, position + consumed, range, out var format);

            placeholder = new ValuePlaceholder<TValueKey>(key, alignment, format);
            return consumed;
        }

        protected int ParserKey<TParseRange>(
            string template, int position, TParseRange range, out TValueKey key)
            where TParseRange : struct, IParseRange
        {
            int start = position;

            while (position < template.Length)
            {
                if (range.IsEndOfRange(template, position))
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

        protected int ParseAlignment<TParseRange>(
            string template, int position, TParseRange range, out int alignment)
            where TParseRange : struct, IParseRange
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
                if (range.IsEndOfRange(template, position))
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

        protected int ParseFormat<TParseRange>(
            string template, int position, TParseRange range, out ReadOnlyMemory<char> format)
            where TParseRange : struct, IParseRange
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
                if (range.IsEndOfRange(template, position))
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
