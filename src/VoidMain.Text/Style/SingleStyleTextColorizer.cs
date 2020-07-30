using System;

namespace VoidMain.Text.Style
{
    public class SingleStyleTextColorizer<TStyle> : ITextColorizer<TStyle>
    {
        private readonly SingleStyleTextColorizerOptions<TStyle> _options;

        public SingleStyleTextColorizer(SingleStyleTextColorizerOptions<TStyle> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            if (_options.SplitSpanByLength < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(_options.SplitSpanByLength));
            }
        }

        public void Colorize<TVisitor>(ReadOnlyMemory<char> text, TVisitor visitor)
            where TVisitor : ITextColorizerVisitor<TStyle>
        {
            int pos = 0;
            int len = _options.SplitSpanByLength;
            var style = _options.Style;

            while (pos + len <= text.Length)
            {
                visitor.Visit(style, text.Slice(pos, len));
                pos += len;
            }

            int rem = text.Length - pos;
            if (rem > 0)
            {
                visitor.Visit(style, text.Slice(pos, rem));
            }
        }
    }
}
