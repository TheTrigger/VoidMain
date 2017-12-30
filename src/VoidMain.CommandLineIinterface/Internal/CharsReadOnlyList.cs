using System;
using System.Collections;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Internal
{
    public class CharsReadOnlyList : IReadOnlyList<char>
    {
        private readonly string _value;

        public string Value => _value;
        public char this[int index] => _value[index];
        public int Count => _value.Length;

        public CharsReadOnlyList(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IEnumerator<char> GetEnumerator() => ((IEnumerable<char>)_value).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_value).GetEnumerator();
    }
}
