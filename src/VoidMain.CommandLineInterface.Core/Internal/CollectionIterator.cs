using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace VoidMain.CommandLineInterface.Internal
{
    [DebuggerDisplay("Index = {Index}, Current = {Current}")]
    public class CollectionIterator<T>
    {
        private readonly IReadOnlyList<T> _collection;
        public int Index { get; private set; }
        public int Count => _collection.Count;

        public CollectionIterator(IReadOnlyList<T> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            Index = 0;
        }

        public bool HasPrev() => Index > 0;
        public bool HasNext() => Index < _collection.Count - 1;

        public bool IsFirst() => Index == 0 && _collection.Count > 0;
        public bool IsLast() => Index == _collection.Count - 1 && _collection.Count > 0;

        public T Current => _collection[Index];

        public bool MoveToPrev()
        {
            bool canMove = HasPrev();
            if (canMove) Index--;
            return canMove;
        }

        public bool MoveToNext()
        {
            bool canMove = HasNext();
            if (canMove) Index++;
            return canMove;
        }

        public void MoveToFirst()
        {
            Index = 0;
        }

        public void MoveToLast()
        {
            Index = _collection.Count - 1;
        }
    }
}
