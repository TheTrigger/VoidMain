using System;

namespace VoidMain.CommandLineInterface.IO.Templates
{
    public interface IMessageTemplateValueFormatter
    {
        string Format(object value, string format,
            IFormatProvider formatProvider, ICustomFormatter customFormatter);
    }
}
