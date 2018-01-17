using System.Collections.Generic;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public class UndoRedoOptions
    {
        public int? MaxCount { get; set; }
        public IEqualityComparer<CommandLineViewSnapshot> SnapshotsComparer { get; set; }
    }
}
