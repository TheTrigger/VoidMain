using System;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public interface IUndoRedoManager<TSnapshot>
    {
        bool TryUndo(TSnapshot currentSnapshot, out TSnapshot prevSnapshot);
        bool TryRedo(TSnapshot currentSnapshot, out TSnapshot nextSnapshot);
        bool TryAddSnapshot(TSnapshot snapshot, bool deleteAfter = true);
        void Clear();
    }
}
