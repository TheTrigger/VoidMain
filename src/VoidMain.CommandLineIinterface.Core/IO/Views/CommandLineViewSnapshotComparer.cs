using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class CommandLineViewSnapshotComparer
    {
        public static IEqualityComparer<CommandLineViewSnapshot> IgnoreCursor =>
            new IgnoreCursorComparer();

        private class IgnoreCursorComparer
            : IEqualityComparer<CommandLineViewSnapshot>
        {
            public bool Equals(CommandLineViewSnapshot x, CommandLineViewSnapshot y)
            {
                return x.LineContent == y.LineContent;
            }

            public int GetHashCode(CommandLineViewSnapshot obj)
            {
                return obj.LineContent?.GetHashCode() ?? 0;
            }
        }
    }
}
