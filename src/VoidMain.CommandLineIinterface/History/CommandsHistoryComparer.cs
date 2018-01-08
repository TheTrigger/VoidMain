using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryEqualityComparer : IEqualityComparer<string>
    {
        public static CommandsHistoryEqualityComparer Ordinal
            => new CommandsHistoryEqualityComparer(StringComparer.Ordinal);
        public static CommandsHistoryEqualityComparer OrdinalIgnoreCase
            => new CommandsHistoryEqualityComparer(StringComparer.OrdinalIgnoreCase);
        public static CommandsHistoryEqualityComparer CurrentCulture
            => new CommandsHistoryEqualityComparer(StringComparer.CurrentCulture);
        public static CommandsHistoryEqualityComparer CurrentCultureIgnoreCase
            => new CommandsHistoryEqualityComparer(StringComparer.CurrentCultureIgnoreCase);

        private readonly StringComparer _comparer;

        protected CommandsHistoryEqualityComparer(StringComparer comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public bool Equals(string x, string y) => _comparer.Equals(x, y);

        public int GetHashCode(string obj) => _comparer.GetHashCode(obj);
    }
}
