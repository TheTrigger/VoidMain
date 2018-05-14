using System;
using VoidMain.CommandLineIinterface.Internal;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public class UndoRedoManager : IUndoRedoManager
    {
        private readonly UndoRedoOptions _options;
        private readonly PushOutCollection<LineViewSnapshot> _snapshots;
        private readonly CollectionIterator<LineViewSnapshot> _iterator;

        public int MaxCount => _options.MaxCount;
        public int Count => _snapshots.Count;

        public UndoRedoManager(UndoRedoOptions options = null)
        {
            _options = options ?? new UndoRedoOptions();
            _options.Validate();
            _snapshots = new PushOutCollection<LineViewSnapshot>(_options.MaxCount);
            _iterator = new CollectionIterator<LineViewSnapshot>(_snapshots);
        }

        private void Validate(ref LineViewSnapshot snapshot, string paramName)
        {
            if (_options.SnapshotsComparer.Equals(snapshot, default(LineViewSnapshot)))
            {
                throw new ArgumentNullException(nameof(paramName));
            }
        }

        public bool TryUndo(LineViewSnapshot currentSnapshot, out LineViewSnapshot prevSnapshot)
        {
            Validate(ref currentSnapshot, nameof(currentSnapshot));

            TryAddSnapshotInternal(ref currentSnapshot);

            if (_iterator.MoveToPrev())
            {
                prevSnapshot = _iterator.Current;
                return true;
            }

            prevSnapshot = default(LineViewSnapshot);
            return false;
        }

        public bool TryRedo(LineViewSnapshot currentSnapshot, out LineViewSnapshot nextSnapshot)
        {
            Validate(ref currentSnapshot, nameof(currentSnapshot));

            TryAddSnapshotInternal(ref currentSnapshot);

            if (_iterator.MoveToNext())
            {
                nextSnapshot = _iterator.Current;
                return true;
            }

            nextSnapshot = default(LineViewSnapshot);
            return false;
        }

        public bool TryAddSnapshot(LineViewSnapshot snapshot)
        {
            Validate(ref snapshot, nameof(snapshot));
            return TryAddSnapshotInternal(ref snapshot);
        }

        private bool TryAddSnapshotInternal(ref LineViewSnapshot snapshot)
        {
            if (EqualsToCurrentSnapshot(ref snapshot))
            {
                return false;
            }

            if (_iterator.HasNext())
            {
                _snapshots.TrimTo(_iterator.Index + 1);
            }

            _snapshots.Add(snapshot);
            _iterator.MoveToLast();

            return true;
        }

        public void Clear()
        {
            _snapshots.Clear();
            _iterator.MoveToFirst();
        }

        private bool EqualsToCurrentSnapshot(ref LineViewSnapshot snapshot)
        {
            return _snapshots.Count > 0
                && _options.SnapshotsComparer.Equals(snapshot, _iterator.Current);
        }
    }
}
