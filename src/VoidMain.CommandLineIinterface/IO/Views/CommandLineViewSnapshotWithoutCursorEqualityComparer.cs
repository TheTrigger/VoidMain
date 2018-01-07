using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class CommandLineViewSnapshotWithoutCursorEqualityComparer : IEqualityComparer<CommandLineViewSnapshot>
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
