using System;
using VoidMain.CommandLineIinterface.UndoRedo;

namespace VoidMain.CommandLineIinterface.Tests.Tools
{
    public class UndoRedoSnapshotInvariantEqualityComparer
        : IUndoRedoSnapshotEqualityComparer<string>
    {
        private readonly StringComparer _comparer = StringComparer.InvariantCulture;

        public bool Equals(string x, string y)
        {
            return _comparer.Equals(x, y);
        }

        public int GetHashCode(string obj)
        {
            return _comparer.GetHashCode(obj);
        }
    }
}
