using System;

namespace VoidMain.Text.Templating.Parser
{
    public struct ValueIndexParser : IValueKeyParser<int>
    {
        public int Parse(ReadOnlySpan<char> span, IFormatProvider formatProvider)
        {
            return Int32.Parse(span, provider: formatProvider);
        }
    }
}
