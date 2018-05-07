using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public interface IUndoRedoManager
    {
        bool TryUndo(CommandLineViewSnapshot currentSnapshot, out CommandLineViewSnapshot prevSnapshot);
        bool TryRedo(CommandLineViewSnapshot currentSnapshot, out CommandLineViewSnapshot nextSnapshot);
        bool TryAddSnapshot(CommandLineViewSnapshot snapshot);
        void Clear();
    }
}
