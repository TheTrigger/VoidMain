using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.IO.Views
{
    public class LineViewSnapshotComparer
    {
        public static IEqualityComparer<LineViewSnapshot> IgnoreCursor =>
            new IgnoreCursorComparer();

        private class IgnoreCursorComparer
            : IEqualityComparer<LineViewSnapshot>
        {
            public bool Equals(LineViewSnapshot x, LineViewSnapshot y)
            {
                return x.Content == y.Content;
            }

            public int GetHashCode(LineViewSnapshot obj)
            {
                return obj.Content?.GetHashCode() ?? 0;
            }
        }
    }
}
