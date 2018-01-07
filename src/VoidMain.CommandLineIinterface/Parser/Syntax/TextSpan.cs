using System;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public struct TextSpan : IEquatable<TextSpan>, IEquatable<string>
    {
        public string Source { get; }
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;
        public bool IsEmpty => Length == 0;
        private string _computedValue;
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
            Source = source;
            Start = start;
            Length = length;
            _computedValue = null;
        }

        public TextSpan(string source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Start = 0;
            Length = source.Length;
            _computedValue = source;
        }

        private string GetText()
        {
            if (IsEmpty) return String.Empty;
            return _computedValue ?? (_computedValue = Source.Substring(Start, Length));
        }

        public static TextSpan RangeInclusive(TextSpan start, TextSpan end)
        {
            if (start.Source == null || end.Source == null
                || end.Source != start.Source)
            {
                throw new ArgumentException(nameof(end),
                    "Source for the end span is not match one for the start span.");
            }

            return new TextSpan(start.Source, start.Start, end.End - start.Start);
        }

        public override string ToString() => GetText();

        public override int GetHashCode()
        {
            return Source == null ? 0 : Source.GetHashCode() ^ Start ^ Length;
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
            if (Source == null || other.Source == null) return false;
            if (Length != other.Length) return false;
            return string.Compare(Source, Start, other.Source, other.Start, Length, comparisonType) == 0;
        }

        public bool Equals(string other)
        {
            return Equals(other, StringComparison.CurrentCulture);
        }

        public bool Equals(string other, StringComparison comparisonType)
        {
            if (other == null) return false;
            if (Source == null) return false;
            if (Length != other.Length) return false;
            return string.Compare(Source, Start, other, 0, Length, comparisonType) == 0;
        }

        public static bool operator ==(TextSpan left, TextSpan right) => left.Equals(right);
        public static bool operator !=(TextSpan left, TextSpan right) => !left.Equals(right);
    }
}
