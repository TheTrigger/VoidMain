﻿using System;
using System.Collections.Generic;
using System.IO;

namespace VoidMain.CommandLineInterface.IO.Templates
{
    public interface IMessageTemplateWriter
    {
        void Write(
            MessageTemplate messageTemplate,
            IReadOnlyList<object> args,
            ITextWriter textWriter,
            IFormatProvider formatProvider = null);
    }
}
