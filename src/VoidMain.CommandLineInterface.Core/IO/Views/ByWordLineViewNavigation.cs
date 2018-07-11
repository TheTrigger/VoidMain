using System;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ByWordLineViewNavigation : ILineViewNavigation
    {
        private static Predicate<char> IsWhitespace = Char.IsWhiteSpace;
        private static Predicate<char> NotWhitespace = c => !Char.IsWhiteSpace(c);

        public int FindNextPosition(IReadOnlyLineView lineView)
        {
            if (lineView.Position >= lineView.Length - 1)
            {
                return lineView.Length;
            }

            int nextPos = lineView.Position + 1;
            char nextSymbol = lineView[nextPos];

            if (IsWhitespace(nextSymbol))
            {
                nextPos = SkipWhileForward(lineView, nextPos, IsWhitespace);
                return SkipWhileForward(lineView, nextPos, NotWhitespace);
            }
            else
            {
                return SkipWhileForward(lineView, nextPos, NotWhitespace);
            }
        }

        public int FindPrevPosition(IReadOnlyLineView lineView)
        {
            if (lineView.Position == 0)
            {
                return 0;
            }

            int prevPos = lineView.Position - 1;
            char prevSymbol = lineView[prevPos];

            if (IsWhitespace(prevSymbol))
            {
                prevPos = SkipWhileBackward(lineView, prevPos, IsWhitespace);
                return SkipWhileBackward(lineView, prevPos, NotWhitespace) + 1;
            }
            else
            {
                return SkipWhileBackward(lineView, prevPos, NotWhitespace) + 1;
            }
        }

        private static int SkipWhileForward(IReadOnlyLineView lineView,
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

        private static int SkipWhileBackward(IReadOnlyLineView lineView,
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
