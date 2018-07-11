using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace VoidMain.CommandLineIinterface.Internal
{
    [DebuggerDisplay(nameof(Count) + " = {" + nameof(Count) + "}")]
    public class CharsReadOnlyList : IReadOnlyList<char>
    {
        public string Value { get; }
        public char this[int index] => Value[index];
        public int Count => Value.Length;

        public CharsReadOnlyList(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IEnumerator<char> GetEnumerator() => ((IEnumerable<char>)Value).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Value).GetEnumerator();
    }
}
