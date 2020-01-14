using System;

namespace VoidMain.IO.Keyboard
{
    [Flags]
    public enum KeyModifiers
    {
        None = 0x0,
        Alt = 0x1,
        Shift = 0x2,
        Control = 0x4
    }
}
