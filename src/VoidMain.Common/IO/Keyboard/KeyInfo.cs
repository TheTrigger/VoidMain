namespace VoidMain.IO.Keyboard
{
    public readonly struct KeyInfo
    {
        public Key Key { get; }
        public KeyModifiers Modifiers { get; }
        public char Character { get; }
        public bool HasMore { get; }

        public KeyInfo(
            Key key, KeyModifiers modifiers,
            char character, bool hasMore)
        {
            Key = key;
            Modifiers = modifiers;
            Character = character;
            HasMore = hasMore;
        }

        public override string ToString()
        {
            return Modifiers == KeyModifiers.None
                ? Key.ToString()
                : $"{Modifiers} + {Key}";
        }
    }
}
