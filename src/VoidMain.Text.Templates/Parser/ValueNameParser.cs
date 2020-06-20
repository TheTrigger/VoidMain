using System;

namespace VoidMain.Text.Templates.Parser
{
    public struct ValueNameParser : IValueKeyParser<string>
    {
        public string Parse(ReadOnlySpan<char> span, IFormatProvider formatProvider)
        {
            return span.ToString();
        }
    }
}
