using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace VoidMain.IO.TextEditors.Internal
{
    [DebuggerDisplay(nameof(Count) + " = {" + nameof(Count) + "}")]
    public class PushOutCollection<T> : IReadOnlyList<T>
    {
        private readonly T[] _elements;
        private int _start;

        public int Count { get; private set; }
        public int Capacity => _elements.Length;

        public T this[int index]
        {
            get => GetElement(index);
            set => GetElement(index) = value;
        }

        public PushOutCollection(int capacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }
            _elements = new T[capacity];
            _start = 0;
            Count = 0;
        }

        private ref T GetElement(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            int offset = (_start + index) % Capacity;
            return ref _elements[offset];
        }

        public void Add(T elem)
        {
            int index = Count;
            if (Count < Capacity)
            {
                Count++;
            }
            else
            {
                _start = (_start + 1) % Capacity;
                index--;
            }
            GetElement(index) = elem;
        }

        public void Clear()
        {
            _start = 0;
            Count = 0;
            Array.Clear(_elements, 0, _elements.Length);
        }

        public void TrimTo(int count)
        {
            if (count == Count) return;
            if (count == 0)
            {
                Clear();
                return;
            }

            if (count < 0 || count > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            Count = count;
            int end = (_start + Count) % Capacity;

            if (end > _start)
            {
                Array.Clear(_elements, end, Capacity - end);
                Array.Clear(_elements, 0, _start);
            }
            else
            {
                Array.Clear(_elements, end, _start - end);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                int offset = (_start + i) % _elements.Length;
                yield return _elements[offset];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
