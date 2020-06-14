using System;

namespace VoidMain.IO.Keyboard
{
    public readonly struct KeyInfo : IEquatable<KeyInfo>
    {
        public Key Key { get; }
        public KeyModifiers Modifiers { get; }

        public KeyInfo(Key key, KeyModifiers modifiers = KeyModifiers.None)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public static readonly KeyInfo AnyKey = default;

        public override string ToString()
        {
            return Modifiers == KeyModifiers.None
                ? Key.ToString()
                : $"{Modifiers} + {Key}";
        }

        public override int GetHashCode()
            => HashCode.Combine(Key, Modifiers);

        public override bool Equals(object? obj)
            => obj is KeyInfo info && Equals(info);

        public bool Equals(KeyInfo other)
            => Key == other.Key && Modifiers == other.Modifiers;

        public static bool operator ==(KeyInfo a, KeyInfo b) => a.Equals(b);

        public static bool operator !=(KeyInfo a, KeyInfo b) => !a.Equals(b);
    }
}
