﻿using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Parser
{
    public class CommandLineSyntaxOptions
    {
        public IEqualityComparer<string> IdentifierComparer { get; set; }

        public static IEqualityComparer<string> DefaultIdentifierComparer
            => StringComparer.OrdinalIgnoreCase;
    }
}
