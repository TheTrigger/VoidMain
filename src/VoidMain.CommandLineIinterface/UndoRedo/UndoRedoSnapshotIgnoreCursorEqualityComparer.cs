using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public class UndoRedoSnapshotIgnoreCursorEqualityComparer
        : IUndoRedoSnapshotEqualityComparer<CommandLineViewSnapshot>
    {
        public bool Equals(CommandLineViewSnapshot x, CommandLineViewSnapshot y)
        {
            return x.LineContent == y.LineContent;
        }

        public int GetHashCode(CommandLineViewSnapshot obj)
        {
            return obj.LineContent == null ? 0 : obj.LineContent.GetHashCode();
        }
    }
}
