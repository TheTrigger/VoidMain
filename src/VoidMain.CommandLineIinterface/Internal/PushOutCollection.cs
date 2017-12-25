﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VoidMain.CommandLineIinterface.Internal
{
    public class PushOutCollection<T> : IEnumerable<T>
    {
        private readonly T[] _elements;
        private int _start;

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

        public PushOutCollection(IEnumerable<T> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }
            _elements = elements.ToArray();

            int capacity = _elements.Length;
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            _start = 0;
            Count = _elements.Length;
        }

        public int Count { get; private set; }
        public int MaxCount => _elements.Length;

        public T this[int index]
        {
            get => GetElement(index);
            set => GetElement(index) = value;
        }

        public void Add(T elem)
        {
            int index = Count;
            if (Count < MaxCount)
            {
                Count++;
            }
            else
            {
                _start = (_start + 1) % MaxCount;
                index--;
            }
            GetElement(index) = elem;
        }

        private ref T GetElement(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            int offset = (_start + index) % MaxCount;
            return ref _elements[offset];
        }

        public void Clear()
        {
            _start = 0;
            Count = 0;
            Array.Clear(_elements, 0, _elements.Length);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}