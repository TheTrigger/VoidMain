using VoidMain.IO.Keyboard;

namespace VoidMain.IO.TextEditors.TextLine
{
    public class TextLineBasicEditingComponentOptions
    {
        public KeyInfo ToggleInsertMode { get; set; } = Key.Insert;
        public KeyInfo DeleteBackward { get; set; } = Key.Backspace;
        public KeyInfo DeleteForward { get; set; } = Key.Delete;
        public KeyInfo DeleteBackwardFast { get; set; } = new KeyInfo(Key.Backspace, KeyModifiers.Control);
        public KeyInfo DeleteForwardFast { get; set; } = new KeyInfo(Key.Delete, KeyModifiers.Control);
        public KeyInfo MoveCursorToStart { get; set; } = Key.Home;
        public KeyInfo MoveCursorToEnd { get; set; } = Key.End;
        public KeyInfo MoveCursorBackward { get; set; } = Key.LeftArrow;
        public KeyInfo MoveCursorForward { get; set; } = Key.RightArrow;
        public KeyInfo MoveCursorBackwardFast { get; set; } = new KeyInfo(Key.LeftArrow, KeyModifiers.Control);
        public KeyInfo MoveCursorForwardFast { get; set; } = new KeyInfo(Key.RightArrow, KeyModifiers.Control);
        public KeyInfo ClearLine { get; set; } = Key.Escape;
        public KeyInfo CloseEditor { get; set; } = Key.Enter;
    }
}
