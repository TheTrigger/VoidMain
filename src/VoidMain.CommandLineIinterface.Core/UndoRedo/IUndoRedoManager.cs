using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public interface IUndoRedoManager
    {
        bool TryUndo(LineViewSnapshot currentSnapshot, out LineViewSnapshot prevSnapshot);
        bool TryRedo(LineViewSnapshot currentSnapshot, out LineViewSnapshot nextSnapshot);
        bool TryAddSnapshot(LineViewSnapshot snapshot);
        void Clear();
    }
}
