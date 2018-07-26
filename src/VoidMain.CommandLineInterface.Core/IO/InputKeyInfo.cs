namespace VoidMain.CommandLineInterface.IO
{
    public class InputKeyInfo
    {
        public InputKey Key { get; }
        public InputModifiers Modifiers { get; }
        public char Character { get; }
        public bool HasMoreInput { get; }

        public InputKeyInfo(
            InputKey key, InputModifiers modifiers,
            char character, bool hasMoreInput)
        {
            Key = key;
            Modifiers = modifiers;
            Character = character;
            HasMoreInput = hasMoreInput;
        }
    }
}
