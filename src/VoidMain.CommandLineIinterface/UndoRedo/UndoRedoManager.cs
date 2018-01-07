using System;
using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Internal;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public class UndoRedoManager<TSnapshot> : IUndoRedoManager<TSnapshot>
    {
        private IEqualityComparer<TSnapshot> _comparer;
        private readonly PushOutCollection<TSnapshot> _snapshots;
        private int _current;

        public int Count => _snapshots.Count;
        public int MaxCount { get; }

        public UndoRedoManager(IEqualityComparer<TSnapshot> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            MaxCount = 10; // TODO: Configure max count.
            _snapshots = new PushOutCollection<TSnapshot>(MaxCount);
            _current = 0;
        }

        public bool TryUndo(TSnapshot currentSnapshot, out TSnapshot prevSnapshot)
        {
            if (_comparer.Equals(currentSnapshot, default(TSnapshot)))
            {
                throw new ArgumentNullException(nameof(currentSnapshot));
            }

            if (IsLast())
            {
                TryAddSnapshot(currentSnapshot, deleteAfter: false);
            }
            else if (!IsEqualsToCurrentSnapshot(currentSnapshot))
            {
                TryAddSnapshot(currentSnapshot, deleteAfter: true);
            }

            if (HasPrev())
            {
                _current--;
                prevSnapshot = _snapshots[_current];
                return true;
            }

            prevSnapshot = default(TSnapshot);
            return false;
        }

        public bool TryRedo(TSnapshot currentSnapshot, out TSnapshot nextSnapshot)
        {
            if (_comparer.Equals(currentSnapshot, default(TSnapshot)))
            {
                throw new ArgumentNullException(nameof(currentSnapshot));
            }

            if (!IsEqualsToCurrentSnapshot(currentSnapshot))
            {
                TryAddSnapshot(currentSnapshot, deleteAfter: true);
            }

            if (HasNext())
            {
                _current++;
                nextSnapshot = _snapshots[_current];
                return true;
            }

            nextSnapshot = default(TSnapshot);
            return false;
        }

        public bool TryAddSnapshot(TSnapshot snapshot, bool deleteAfter = true)
        {
            if (_comparer.Equals(snapshot, default(TSnapshot)))
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            if (deleteAfter && HasNext())
            {
                _snapshots.TrimTo(_current + 1);
            }

            if (IsEqualsToLastSnapshot(snapshot))
            {
                return false;
            }

            _snapshots.Add(snapshot);
            _current = _snapshots.Count - 1;

            return true;
        }

        public void Clear()
        {
            _snapshots.Clear();
            _current = 0;
        }

        private bool IsLast()
        {
            return _current == _snapshots.Count - 1;
        }

        private bool HasNext()
        {
            return _current < _snapshots.Count - 1;
        }

        private bool HasPrev()
        {
            return _current > 0;
        }

        private bool IsEqualsToLastSnapshot(TSnapshot snapshot)
        {
            return _snapshots.Count > 0
                && _comparer.Equals(snapshot, _snapshots[_snapshots.Count - 1]);
        }

        private bool IsEqualsToCurrentSnapshot(TSnapshot snapshot)
        {
            return _current >= 0 && _current < _snapshots.Count
                && _comparer.Equals(snapshot, _snapshots[_current]);
        }
    }
}
