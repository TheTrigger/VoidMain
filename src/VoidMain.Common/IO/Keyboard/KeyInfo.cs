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

        public void Deconstruct(out Key key, out KeyModifiers modifiers)
        {
            key = Key;
            modifiers = Modifiers;
        }

        public static readonly KeyInfo NoKey = default;

        public static readonly KeyInfo AnyKey = Key.Any;

        public override string ToString()
        {
            return Modifiers == KeyModifiers.None
                ? Key.ToString()
                : $"{Modifiers} + {Key}";
        }

        public override int GetHashCode()
            => (byte)Key | (byte)Modifiers << 8;

        public override bool Equals(object? obj)
            => obj is KeyInfo info && Equals(info);

        public bool Equals(KeyInfo other)
            => Key == other.Key && Modifiers == other.Modifiers;

        public static bool operator ==(KeyInfo a, KeyInfo b) => a.Equals(b);

        public static bool operator !=(KeyInfo a, KeyInfo b) => !a.Equals(b);

        public static implicit operator KeyInfo(Key key) => new KeyInfo(key);
    }
}
