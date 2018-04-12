using System;
using System.Collections.Generic;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.UndoRedo
{
    public class UndoRedoOptions
    {
        public int MaxCount { get; set; }
        public IEqualityComparer<CommandLineViewSnapshot> SnapshotsComparer { get; set; }

        public UndoRedoOptions(bool defaults = true)
        {
            if (defaults)
            {
                MaxCount = 10;
                SnapshotsComparer = CommandLineViewSnapshotComparer.IgnoreCursor;
            }
        }

        public void Validate()
        {
            if (MaxCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxCount));
            }
            if (SnapshotsComparer == null)
            {
                throw new ArgumentNullException(nameof(SnapshotsComparer));
            }
        }
    }
}
