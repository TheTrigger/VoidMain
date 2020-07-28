using System;

namespace VoidMain.IO.Keyboard
{
    public readonly struct KeyInput : IEquatable<KeyInput>
    {
        public KeyInfo KeyInfo { get; }
        public char Character { get; }
        public bool IsNextKeyAvailable { get; }

        public KeyInput(KeyInfo keyInfo, char character, bool isNextKeyAvailable)
        {
            KeyInfo = keyInfo;
            Character = character;
            IsNextKeyAvailable = isNextKeyAvailable;
        }

        public override string ToString() => KeyInfo.ToString();

        public override int GetHashCode()
            => HashCode.Combine(KeyInfo, Character, IsNextKeyAvailable);

        public override bool Equals(object? obj)
            => obj is KeyInput input && Equals(input);

        public bool Equals(KeyInput other)
            => KeyInfo == other.KeyInfo && Character == other.Character && IsNextKeyAvailable == other.IsNextKeyAvailable;

        public static bool operator ==(KeyInput a, KeyInput b) => a.Equals(b);

        public static bool operator !=(KeyInput a, KeyInput b) => !a.Equals(b);
    }
}
