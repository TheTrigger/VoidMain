using System;
using System.Collections.Generic;
using VoidMain.CommandLineInterface.IO.Views;

namespace VoidMain.CommandLineInterface.UndoRedo
{
    public class UndoRedoOptions
    {
        public int MaxCount { get; set; }
        public IEqualityComparer<LineViewSnapshot> SnapshotsComparer { get; set; }

        public UndoRedoOptions(bool defaults = true)
        {
            if (defaults)
            {
                MaxCount = 10;
                SnapshotsComparer = LineViewSnapshotComparer.IgnoreCursor;
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
