﻿using System;
using System.Collections.Generic;
using System.IO;

namespace VoidMain.CommandLineInterface.IO.Templates
{
    public class MessageTemplateWriter : IMessageTemplateWriter
    {
        private readonly Type CustomFormatterType = typeof(ICustomFormatter);

        private readonly IMessageTemplateValueFormatter _valueFormatter;

        public MessageTemplateWriter(IMessageTemplateValueFormatter valueFormatter)
        {
            _valueFormatter = valueFormatter ?? throw new ArgumentNullException(nameof(valueFormatter));
        }

        public void Write(
            MessageTemplate messageTemplate,
            IReadOnlyList<object> args,
            ITextWriter textWriter,
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

            foreach (var token in messageTemplate.Tokens)
            {
                switch (token)
                {
                    case MessageTemplate.TextToken text:
                        textWriter.Write(text.Text);
                        break;
                    case MessageTemplate.ArgumentToken arg:
                        var value = args[arg.Index];
                        string formatedValue = _valueFormatter.Format(value, arg.Format, formatProvider, formatter);
                        WriteValueWithPadding(textWriter, arg, formatedValue);
                        break;
                    default:
                        throw new FormatException($"Unknown format token `{token?.GetType().Name}`.");
                }
            }
        }

        private void WriteValueWithPadding(
            ITextWriter textWriter,
            MessageTemplate.ArgumentToken arg,
            string formatedValue)
        {
            if (arg.Alignment > 0)
            {
                int padLeft = arg.Alignment - formatedValue.Length;
                textWriter.Write(' ', padLeft);
            }

            textWriter.Write(formatedValue);

            if (arg.Alignment < 0)
            {
                int padRight = -arg.Alignment - formatedValue.Length;
                textWriter.Write(' ', padRight);
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
