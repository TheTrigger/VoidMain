using System;

namespace VoidMain.Text.Templates.Formatter
{
    public interface IValueFormatter
    {
        bool TryFormatToSpan(object value, ReadOnlySpan<char> format,
            IFormatProvider formatProvider, Span<char> destination, out int charsWritten);

        string Format(object value, ReadOnlySpan<char> format,
            IFormatProvider formatProvider, ICustomFormatter customFormatter);
    }
}
