using System;

namespace VoidMain.CommandLineIinterface.IO.Internal
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

        public static bool HasNoModifiers(this ConsoleKeyInfo keyInfo)
        {
            return keyInfo.Modifiers == 0;
        }
    }
}
