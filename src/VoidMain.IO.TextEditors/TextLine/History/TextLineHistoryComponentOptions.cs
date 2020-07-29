using VoidMain.IO.Keyboard;

namespace VoidMain.IO.TextEditors.TextLine.History
{
    public class TextLineHistoryComponentOptions
    {
        public KeyInfo Prev { get; set; } = Key.UpArrow;
        public KeyInfo Next { get; set; } = Key.DownArrow;
        public int Capacity { get; set; } = 10;
    }
}
