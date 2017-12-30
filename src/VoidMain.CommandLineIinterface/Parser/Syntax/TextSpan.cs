using System;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public struct TextSpan : IEquatable<TextSpan>, IEquatable<string>
    {
        public static readonly TextSpan Empty = new TextSpan();

        private readonly string _source;
        private readonly int _start;
        private readonly int _length;
        private string _computedValue;

        public string Source => _source;
        public int Start => _start;
        public int Length => _length;
        public int End => _start + _length;
        public bool IsEmpty => _length == 0;
        public string Text => GetText();

        public TextSpan(string source, int start, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (start < 0 || start > source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if (length < 0 || start + length > source.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            _source = source;
            _start = start;
            _length = length;
            _computedValue = null;
        }

        private string GetText()
        {
            if (IsEmpty) return String.Empty;
            return _computedValue ?? (_computedValue = _source.Substring(_start, _length));
        }

        public static TextSpan RangeInclusive(TextSpan start, TextSpan end)
        {
            if (start._source != end._source)
            {
                throw new ArgumentException(nameof(end),
                    "Source for end span is not match one for start span.");
            }

            return new TextSpan(start.Source, start.Start, end.End - start.Start);
        }

        public override string ToString() => GetText();

        public override int GetHashCode()
        {
            return _source == null ? 0 : _source.GetHashCode() ^ _start ^ _length;
        }

        public override bool Equals(object obj)
        {
            return (obj is TextSpan span && Equals(span))
                || (obj is string str && Equals(str));
        }

        public bool Equals(TextSpan other)
        {
            return Equals(other, StringComparison.CurrentCulture);
        }

        public bool Equals(TextSpan other, StringComparison comparisonType)
        {
            if (_source == null || other._source == null) return false;
            if (_length != other._length) return false;
            return string.Compare(_source, _start, other._source, other._start, _length, comparisonType) == 0;
        }

        public bool Equals(string other)
        {
            return Equals(other, StringComparison.CurrentCulture);
        }

        public bool Equals(string other, StringComparison comparisonType)
        {
            if (other == null) return false;
            if (_source == null) return false;
            if (_length != other.Length) return false;
            return string.Compare(_source, _start, other, 0, _length, comparisonType) == 0;
        }

        public static bool operator ==(TextSpan left, TextSpan right) => left.Equals(right);

        public static bool operator !=(TextSpan left, TextSpan right) => !left.Equals(right);
    }
}
