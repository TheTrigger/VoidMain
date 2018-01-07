using System;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryOrdinalIgnoreCaseEqualityComparer : CommandsHistoryEqualityComparer
    {
        public CommandsHistoryOrdinalIgnoreCaseEqualityComparer()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
