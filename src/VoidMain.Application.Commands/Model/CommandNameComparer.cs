using System;
using System.Collections.Generic;

namespace VoidMain.Application.Commands.Model
{
    public class CommandNameComparer : IEqualityComparer<CommandName>
    {
        private readonly IEqualityComparer<string> _comparer;

        public CommandNameComparer(IEqualityComparer<string> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public int GetHashCode(CommandName obj)
        {
            if (obj == null) return 0;

            int hash = 0;
            var parts = obj.Parts;

            for (int i = 0; i < parts.Count; i++)
            {
                hash = 31 * hash + _comparer.GetHashCode(parts[i]);
            }

            return hash;
        }

        public bool Equals(CommandName x, CommandName y)
        {
            if (ReferenceEquals(x, null))
            {
                return ReferenceEquals(y, null);
            }

            if (ReferenceEquals(x, y)) return true;

            var a = x.Parts;
            var b = y.Parts;

            if (a.Count != b.Count) return false;

            for (int i = 0; i < a.Count; i++)
            {
                if (!_comparer.Equals(a[i], b[i])) return false;
            }

            return true;
        }

        public bool StartsWith(CommandName x, CommandName y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }
            return StartsWith(x.Parts, y.Parts);
        }

        public bool StartsWith(CommandName x, IReadOnlyList<string> y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }
            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }
            return StartsWith(x.Parts, y);
        }

        private bool StartsWith(IReadOnlyList<string> x, IReadOnlyList<string> y)
        {
            if (y.Count > x.Count) return false;

            for (int i = 0; i < x.Count; i++)
            {
                if (!_comparer.Equals(x[i], y[i])) return false;
            }

            return true;
        }
    }
}
