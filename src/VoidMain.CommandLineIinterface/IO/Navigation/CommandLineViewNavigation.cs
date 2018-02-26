using System;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO.Navigation
{
    public class CommandLineViewNavigation : ICommandLineViewNavigation
    {
        private static Predicate<char> IsWhitespace = Char.IsWhiteSpace;
        private static Predicate<char> IsNotWhitespace = c => !Char.IsWhiteSpace(c);

        public int FindNextPosition(ICommandLineReadOnlyView lineView)
        {
            if (lineView.Position >= lineView.Length - 1)
            {
                return lineView.Length;
            }

            int currPos = lineView.Position + 1;
            char nextSymbol = lineView[currPos];

            var predicate = IsWhitespace(nextSymbol)
                ? IsWhitespace
                : IsNotWhitespace;
            return SkipWhileForward(lineView, currPos, predicate);
        }

        public int FindPrevPosition(ICommandLineReadOnlyView lineView)
        {
            if (lineView.Position == 0) return 0;

            int currPos = lineView.Position - 1;
            char prevSymbol = lineView[currPos];

            var predicate = IsWhitespace(prevSymbol)
                ? IsWhitespace
                : IsNotWhitespace;
            return SkipWhileBackward(lineView, currPos, predicate) + 1;
        }

        private static int SkipWhileForward(ICommandLineReadOnlyView lineView,
            int start, Predicate<char> predicate)
        {
            int pos;
            for (pos = start; pos < lineView.Length; pos++)
            {
                char symbol = lineView[pos];
                if (!predicate(symbol)) break;
            }
            return pos;
        }

        private static int SkipWhileBackward(ICommandLineReadOnlyView lineView,
            int start, Predicate<char> predicate)
        {
            int pos;
            for (pos = start; pos >= 0; pos--)
            {
                char symbol = lineView[pos];
                if (!predicate(symbol)) break;
            }
            return pos;
        }
    }
}
