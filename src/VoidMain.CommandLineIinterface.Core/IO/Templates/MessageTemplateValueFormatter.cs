using System;

namespace VoidMain.CommandLineIinterface.IO.Templates
{
    public class MessageTemplateValueFormatter : IMessageTemplateValueFormatter
    {
        public string Format(object value, string format,
            IFormatProvider formatProvider, ICustomFormatter customFormatter)
        {
            string result = null;

            if (customFormatter != null)
            {
                result = customFormatter.Format(format, value, formatProvider);
            }

            if (result == null)
            {
                if (value is IFormattable formattable)
                {
                    result = formattable.ToString(format, formatProvider);
                }
                else
                {
                    result = value?.ToString() ?? String.Empty;
                }
            }

            return result;
        }
    }
}
