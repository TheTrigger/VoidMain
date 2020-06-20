using System;

namespace VoidMain.Text.Templates.Formatter
{
    public struct TextTemplateFormatterVisitor<TValueKey, TValueSource>
        : ITextTemplateVisitor<ValuePlaceholder<TValueKey>>
        where TValueSource : IValueSource<TValueKey>
    {
        private static readonly Type CustomFormatterType = typeof(ICustomFormatter);

        private ITextWriter _textWriter;
        private TValueSource _valueSource;
        private IValueFormatter _valueFormatter;
        private IFormatProvider _formatProvider;
        private ICustomFormatter _customFormatter;

        public TextTemplateFormatterVisitor(
            ITextWriter textWriter,
            TValueSource valueSource,
            IValueFormatter valueFormatter,
            IFormatProvider formatProvider = null)
        {
            _textWriter = textWriter ?? throw new ArgumentNullException(nameof(textWriter));
            _valueSource = valueSource ?? throw new ArgumentNullException(nameof(valueSource));
            _valueFormatter = valueFormatter ?? throw new ArgumentNullException(nameof(valueFormatter));
            _formatProvider = formatProvider;
            _customFormatter = GetFormatter(_formatProvider);
        }

        public void BeforeVisitAll() { }

        public void AfterVisitAll() { }

        public void Visit(ReadOnlyMemory<char> text)
        {
            _textWriter.Write(text.Span);
        }

        public void Visit(ValuePlaceholder<TValueKey> placeholder)
        {
            var value = _valueSource.GetValue(placeholder.Key);

            Span<char> buffer = stackalloc char[64];
            if (_valueFormatter.TryFormatToSpan(value, placeholder.Format.Span, _formatProvider, buffer, out int charsWritten))
            {
                WriteValueWithPadding(placeholder.Alignment, buffer.Slice(0, charsWritten));
                return;
            }

            string formatedValue = _valueFormatter.Format(value, placeholder.Format.ToString(), _formatProvider, _customFormatter);
            WriteValueWithPadding(placeholder.Alignment, formatedValue.AsSpan());
        }

        private void WriteValueWithPadding(int alignment, ReadOnlySpan<char> formatedValue)
        {
            if (alignment > 0)
            {
                int padding = alignment - formatedValue.Length;
                _textWriter.Write(' ', padding);
            }

            _textWriter.Write(formatedValue);

            if (alignment < 0)
            {
                int padding = -alignment - formatedValue.Length;
                _textWriter.Write(' ', padding);
            }
        }

        private static ICustomFormatter GetFormatter(IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                return null;
            }
            return (ICustomFormatter)formatProvider.GetFormat(CustomFormatterType);
        }
    }
}
