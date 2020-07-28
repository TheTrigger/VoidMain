using System;
using System.Collections.Generic;

namespace VoidMain.Text.Templates
{
    public readonly struct ValuePlaceholder<TValueKey> : IEquatable<ValuePlaceholder<TValueKey>>
    {
        public TValueKey Key { get; }
        public int Alignment { get; }
        public ReadOnlyMemory<char> Format { get; }

        public ValuePlaceholder(
            TValueKey key,
            int alignment = default,
            ReadOnlyMemory<char> format = default)
        {
            Key = key;
            Alignment = alignment;
            Format = format;
        }

        public override string ToString() => Alignment == 0
            ? (Format.IsEmpty ? $"{Key}" : $"{Key}:{Format}")
            : (Format.IsEmpty ? $"{Key},{Alignment}" : $"{Key},{Alignment}:{Format}");

        public bool Equals(ValuePlaceholder<TValueKey> other)
        {
            var keyComparer = EqualityComparer<TValueKey>.Default;

            return keyComparer.Equals(Key, other.Key)
                && Alignment.Equals(other.Alignment)
                && Format.Equals(other.Format);
        }

        public override bool Equals(object obj)
        {
            return obj is ValuePlaceholder<TValueKey> placeholder && Equals(placeholder);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();

            if (Key != null)
            {
                hashCode.Add(Key);
            }

            hashCode.Add(Alignment);
            hashCode.Add(Format);

            return hashCode.ToHashCode();
        }

        public static implicit operator ValuePlaceholder<TValueKey>(TValueKey key)
        {
            return new ValuePlaceholder<TValueKey>(key);
        }
    }
}
