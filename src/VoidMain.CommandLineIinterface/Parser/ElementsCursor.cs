using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Parser
{
    public class ElementsCursor<T>
    {
        private int _position;
        private readonly IReadOnlyList<T> _elements;
        private readonly T _terminalElement;

        public int Position => _position;
        public IReadOnlyList<T> Elements => _elements;

        public ElementsCursor(IReadOnlyList<T> elements, T terminalElement)
        {
            _elements = elements ?? throw new ArgumentNullException(nameof(elements));
            _terminalElement = terminalElement;
            _position = 0;
        }

        /// <summary>
        /// Is cusor at the end.
        /// </summary>
        /// <returns></returns>
        public bool IsAtTheEnd()
        {
            return _position >= _elements.Count;
        }

        /// <summary>
        /// Move the cursor to the next element and return it.
        /// </summary>
        public T GetNext()
        {
            MoveNext();
            return Peek();
        }

        /// <summary>
        /// Move the cursor to the next element.
        /// </summary>
        public void MoveNext(int delta = 1)
        {
            _position += delta;
        }

        /// <summary>
        /// Get the element without moving the cursor.
        /// </summary>
        public T Peek(int delta = 0)
        {
            int peekPosition = _position + delta;
            if (peekPosition >= _elements.Count)
            {
                return _terminalElement;
            }
            return _elements[peekPosition];
        }

        public override string ToString()
        {
            var current = Peek();
            var next = Peek(1);
            return $"{current} => {next}";
        }
    }
}
