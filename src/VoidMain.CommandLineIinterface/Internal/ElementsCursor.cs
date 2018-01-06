using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Internal
{
    public class ElementsCursor<T>
    {
        public int Position { get; private set; }
        public IReadOnlyList<T> Elements { get; }
        public T TerminalElement { get; }

        public ElementsCursor(IReadOnlyList<T> elements, T terminalElement)
        {
            Elements = elements ?? throw new ArgumentNullException(nameof(elements));
            TerminalElement = terminalElement;
            Position = 0;
        }

        /// <summary>
        /// Is cusor at the end.
        /// </summary>
        /// <returns></returns>
        public bool IsAtTheEnd()
        {
            return Position >= Elements.Count;
        }

        /// <summary>
        /// Move the cursor to the next element.
        /// </summary>
        public void MoveNext(int delta = 1)
        {
            Position += delta;
        }

        /// <summary>
        /// Get the element without moving the cursor.
        /// </summary>
        public T Peek(int delta = 0)
        {
            int peekPosition = Position + delta;
            if (peekPosition >= Elements.Count)
            {
                return TerminalElement;
            }
            return Elements[peekPosition];
        }

        public override string ToString()
        {
            var current = Peek();
            var next = Peek(1);
            return $"{current} => {next}";
        }
    }
}
