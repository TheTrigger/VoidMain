using System;

namespace VoidMain.CommandLineIinterface.History
{
    public abstract class CommandsHistoryEqualityComparer : ICommandsHistoryEqualityComparer
    {
        private readonly StringComparer _comparer;

        protected CommandsHistoryEqualityComparer(StringComparer comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public bool Equals(string x, string y) => _comparer.Equals(x, y);

        public int GetHashCode(string obj) => _comparer.GetHashCode(obj);
    }
}
