using System;

namespace VoidMain.Text.Templating.Parser
{
    public interface IValueKeyParser<TKey>
    {
        TKey Parse(ReadOnlySpan<char> span, IFormatProvider formatProvider);
    }
}
