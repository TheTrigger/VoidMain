namespace VoidMain.CommandLineIinterface.IO.Internal
{
    public static class ConsoleExtensions
    {
        public static void Write(this IConsole console, char symbol, int length)
        {
            for (int i = 0; i < length; i++)
            {
                console.Write(symbol);
            }
        }
    }
}
