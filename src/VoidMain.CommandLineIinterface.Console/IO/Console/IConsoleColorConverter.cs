using System;

namespace VoidMain.CommandLineIinterface.IO.Console
{
    public interface IConsoleColorConverter
    {
        bool TryConvert(Color color, out ConsoleColor consoleColor);
    }
}
