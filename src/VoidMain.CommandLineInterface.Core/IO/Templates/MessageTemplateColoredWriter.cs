using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.IO.Templates
{
    public class MessageTemplateColoredWriter : IMessageTemplateColoredWriter
    {
        private readonly Type CustomFormatterType = typeof(ICustomFormatter);

        private readonly IMessageTemplateValueFormatter _valueFormatter;

        public MessageTemplateColoredWriter(IMessageTemplateValueFormatter valueFormatter)
        {
            _valueFormatter = valueFormatter ?? throw new ArgumentNullException(nameof(valueFormatter));
        }

        public void Write(
            Colored<MessageTemplate> messageTemplate,
            IReadOnlyList<Colored<object>> args,
            IColoredTextWriter textWriter,
            IFormatProvider formatProvider = null)
        {
            if (messageTemplate == null)
            {
                throw new ArgumentNullException(nameof(messageTemplate));
            }
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            if (textWriter == null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            var formatter = GetFormatter(formatProvider);

            var textForegroud = messageTemplate.Foreground;
            var textBackground = messageTemplate.Background;

            foreach (var token in messageTemplate.Value.Tokens)
            {
                switch (token)
                {
                    case MessageTemplate.TextToken text:
                        textWriter.Write(textForegroud, textBackground, text.Text);
                        break;
                    case MessageTemplate.ArgumentToken arg:
                        var coloredValue = args[arg.Index];
                        string formatedValue = _valueFormatter.Format(coloredValue.Value, arg.Format, formatProvider, formatter);
                        WriteValueWithPadding(
                            textWriter, arg, formatedValue,
                            textForegroud, textBackground,
                            coloredValue.Foreground, coloredValue.Background);
                        break;
                    default:
                        throw new FormatException($"Unknown format token `{token?.GetType().Name}`.");
                }
            }
        }

        private void WriteValueWithPadding(
            IColoredTextWriter textWriter,
            MessageTemplate.ArgumentToken arg,
            string formatedValue,
            Color? textForegroud,
            Color? textBackground,
            Color? valueForeground,
            Color? valueBackground)
        {
            if (arg.Alignment > 0)
            {
                int padLeft = arg.Alignment - formatedValue.Length;
                textWriter.Write(textForegroud, textBackground, ' ', padLeft);
            }

            textWriter.Write(
                valueForeground ?? textForegroud,
                valueBackground ?? textBackground,
                formatedValue);

            if (arg.Alignment < 0)
            {
                int padRight = -arg.Alignment - formatedValue.Length;
                textWriter.Write(textForegroud, textBackground, ' ', padRight);
            }
        }

        private ICustomFormatter GetFormatter(IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                return null;
            }
            return (ICustomFormatter)formatProvider.GetFormat(CustomFormatterType);
        }
    }
}
