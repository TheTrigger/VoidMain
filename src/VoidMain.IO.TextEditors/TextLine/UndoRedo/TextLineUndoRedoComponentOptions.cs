using System;
using VoidMain.IO.Keyboard;

namespace VoidMain.IO.TextEditors.TextLine.UndoRedo
{
    public class TextLineUndoRedoComponentOptions
    {
        public KeyInfo Undo { get; set; } = new KeyInfo(Key.Z, KeyModifiers.Control);
        public KeyInfo Redo { get; set; } = new KeyInfo(Key.Y, KeyModifiers.Control);
        public int MaxSteps { get; set; } = 32;
        public TimeSpan AccumulationPeriod { get; set; } = TimeSpan.Zero;
    }
}
