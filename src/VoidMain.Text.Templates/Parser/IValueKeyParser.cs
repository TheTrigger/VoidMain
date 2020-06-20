using System;

namespace VoidMain.Text.Templates.Parser
{
    public interface IValueKeyParser<TKey>
    {
        TKey Parse(ReadOnlySpan<char> span, IFormatProvider formatProvider);
    }
}
