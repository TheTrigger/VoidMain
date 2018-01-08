using System;
using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Internal;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public class UndoRedoOptions
    {
        public int? MaxCount { get; set; }
        public IEqualityComparer<CommandLineViewSnapshot> SnapshotsComparer { get; set; }
    }

    public class UndoRedoManager : IUndoRedoManager
    {
        private readonly IEqualityComparer<CommandLineViewSnapshot> _comparer;
        private readonly PushOutCollection<CommandLineViewSnapshot> _snapshots;
        private int _current;

        public int MaxCount { get; }
        public int Count => _snapshots.Count;

        public UndoRedoManager(UndoRedoOptions options = null)
        {
            _comparer = options?.SnapshotsComparer ?? CommandLineViewSnapshotComparer.IgnoreCursor;
            MaxCount = options?.MaxCount ?? 10;
            if (MaxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxCount));
            }
            _snapshots = new PushOutCollection<CommandLineViewSnapshot>(MaxCount);
            _current = 0;
        }

        public bool TryUndo(CommandLineViewSnapshot currentSnapshot, out CommandLineViewSnapshot prevSnapshot)
        {
            if (_comparer.Equals(currentSnapshot, default(CommandLineViewSnapshot)))
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

            prevSnapshot = default(CommandLineViewSnapshot);
            return false;
        }

        public bool TryRedo(CommandLineViewSnapshot currentSnapshot, out CommandLineViewSnapshot nextSnapshot)
        {
            if (_comparer.Equals(currentSnapshot, default(CommandLineViewSnapshot)))
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

            nextSnapshot = default(CommandLineViewSnapshot);
            return false;
        }

        public bool TryAddSnapshot(CommandLineViewSnapshot snapshot, bool deleteAfter = true)
        {
            if (_comparer.Equals(snapshot, default(CommandLineViewSnapshot)))
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

        private bool IsEqualsToLastSnapshot(CommandLineViewSnapshot snapshot)
        {
            return _snapshots.Count > 0
                && _comparer.Equals(snapshot, _snapshots[_snapshots.Count - 1]);
        }

        private bool IsEqualsToCurrentSnapshot(CommandLineViewSnapshot snapshot)
        {
            return _current >= 0 && _current < _snapshots.Count
                && _comparer.Equals(snapshot, _snapshots[_current]);
        }
    }
}
