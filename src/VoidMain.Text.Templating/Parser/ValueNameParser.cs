using System;

namespace VoidMain.Text.Templating.Parser
{
    public struct ValueNameParser : IValueKeyParser<string>
    {
        public string Parse(ReadOnlySpan<char> span, IFormatProvider formatProvider)
        {
            return span.ToString();
        }
    }
}
