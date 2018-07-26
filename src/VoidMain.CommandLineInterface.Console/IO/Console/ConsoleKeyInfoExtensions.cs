namespace VoidMain.CommandLineInterface.IO.Console
{
    public static class InputKeyInfoExtensions
    {
        public static bool HasAltKey(this InputKeyInfo keyInfo)
        {
            return (keyInfo.Modifiers & InputModifiers.Alt) == InputModifiers.Alt;
        }

        public static bool HasShiftKey(this InputKeyInfo keyInfo)
        {
            return (keyInfo.Modifiers & InputModifiers.Shift) == InputModifiers.Shift;
        }

        public static bool HasControlKey(this InputKeyInfo keyInfo)
        {
            return (keyInfo.Modifiers & InputModifiers.Control) == InputModifiers.Control;
        }

        public static bool HasNoModifiers(this InputKeyInfo keyInfo)
        {
            return keyInfo.Modifiers == 0;
        }
    }
}
