using System;
using VoidMain.Text.Style;

namespace VoidMain.Text.Templates.Parser
{
    public class TextStyleParser : IStyleParser<TextStyle>
    {
        public int Parse<TParseRange>(string template, int position, TParseRange range, out TextStyle style)
            where TParseRange : struct, IParseRange
        {
            int totalConsumed = 0;

            Color? foreground = null;
            Color? background = null;
            Color color;

            int consumed = ParseColor(template, position, range, out color);
            if (consumed > 0)
            {
                foreground = color;
                totalConsumed += consumed;
                position += consumed;
            }

            bool hasBackground = position < template.Length
                && !range.IsEndOfRange(template, position);

            if (hasBackground)
            {
                totalConsumed++;
                position++;

                consumed = ParseColor(template, position, range, out color);
                if (consumed > 0)
                {
                    background = color;
                    totalConsumed += consumed;
                }
            }

            style = new TextStyle(foreground, background);
            return totalConsumed;
        }

        private int ParseColor<TParseRange>(string template, int position, TParseRange range, out Color color)
            where TParseRange : struct, IParseRange
        {
            int start = position;

            while (position < template.Length)
            {
                if (range.IsEndOfRange(template, position)) break;
                if (template[position] == ',') break;
                position++;
            }

            if (position == start)
            {
                color = default;
                return 0;
            }

            int consumed = position - start;
            var rawKey = template.AsSpan(start, consumed);

            if (Color.TryParse(rawKey, out color))
            {
                return consumed;
            }

            throw new FormatException();
        }
    }
}
