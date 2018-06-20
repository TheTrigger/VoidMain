using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.IO.Templates
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
