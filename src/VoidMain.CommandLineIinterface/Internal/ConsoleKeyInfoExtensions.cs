using System;

namespace VoidMain.CommandLineIinterface.Internal
{
    public static class ConsoleKeyInfoExtensions
    {
        public static bool HasAltKey(this ConsoleKeyInfo keyInfo)
        {
            return keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt);
        }

        public static bool HasShiftKey(this ConsoleKeyInfo keyInfo)
        {
            return keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift);
        }

        public static bool HasControlKey(this ConsoleKeyInfo keyInfo)
        {
            return keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control);
        }
    }
}
