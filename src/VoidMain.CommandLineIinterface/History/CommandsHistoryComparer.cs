using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryComparer : IEqualityComparer<string>
    {
        public static CommandsHistoryComparer Ordinal
            => new CommandsHistoryComparer(StringComparer.Ordinal);
        public static CommandsHistoryComparer OrdinalIgnoreCase
            => new CommandsHistoryComparer(StringComparer.OrdinalIgnoreCase);
        public static CommandsHistoryComparer CurrentCulture
            => new CommandsHistoryComparer(StringComparer.CurrentCulture);
        public static CommandsHistoryComparer CurrentCultureIgnoreCase
            => new CommandsHistoryComparer(StringComparer.CurrentCultureIgnoreCase);

        private readonly StringComparer _comparer;

        protected CommandsHistoryComparer(StringComparer comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public bool Equals(string x, string y) => _comparer.Equals(x, y);

        public int GetHashCode(string obj) => _comparer.GetHashCode(obj);
    }
}
