using System;

namespace VoidMain.IO.Keyboard
{
    public readonly struct KeyInput : IEquatable<KeyInput>
    {
        public KeyInfo KeyInfo { get; }
        public char Character { get; }
        public bool HasMore { get; }

        public KeyInput(KeyInfo keyInfo, char character, bool hasMore)
        {
            KeyInfo = keyInfo;
            Character = character;
            HasMore = hasMore;
        }

        public override string ToString() => KeyInfo.ToString();

        public override int GetHashCode()
            => HashCode.Combine(KeyInfo, Character, HasMore);

        public override bool Equals(object? obj)
            => obj is KeyInput input && Equals(input);

        public bool Equals(KeyInput other)
            => KeyInfo == other.KeyInfo && Character == other.Character && HasMore == other.HasMore;

        public static bool operator ==(KeyInput a, KeyInput b) => a.Equals(b);

        public static bool operator !=(KeyInput a, KeyInput b) => !a.Equals(b);
    }
}
