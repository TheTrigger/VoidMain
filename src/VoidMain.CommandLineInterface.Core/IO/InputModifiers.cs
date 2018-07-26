using System;

namespace VoidMain.CommandLineInterface.IO
{
    [Flags]
    public enum InputModifiers
    {
        None = 0x0,
        Alt = 0x1,
        Shift = 0x2,
        Control = 0x4
    }
}
