using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface
{
    public class CommandLineOptions
    {
        public IEqualityComparer<string> IdentifierComparer { get; set; }

        public CommandLineOptions(bool defaults = true)
        {
            if (defaults)
            {
                IdentifierComparer = StringComparer.OrdinalIgnoreCase;
            }
        }

        public void Validate()
        {
            if (IdentifierComparer == null)
            {
                throw new ArgumentNullException(nameof(IdentifierComparer));
            }
        }
    }
}
