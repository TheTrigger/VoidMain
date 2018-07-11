using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.IO.Templates
{
    public interface IMessageTemplateColoredWriter
    {
        void Write(
            Colored<MessageTemplate> messageTemplate,
            IReadOnlyList<Colored<object>> args,
            IColoredTextWriter textWriter,
            IFormatProvider formatProvider = null);
    }
}
