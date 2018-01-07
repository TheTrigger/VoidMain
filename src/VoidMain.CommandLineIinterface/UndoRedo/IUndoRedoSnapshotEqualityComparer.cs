using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public interface IUndoRedoSnapshotEqualityComparer<TSnapshot>
        : IEqualityComparer<TSnapshot>
    { }
}
