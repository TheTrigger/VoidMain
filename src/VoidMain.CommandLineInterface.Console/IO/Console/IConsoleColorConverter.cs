using System;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public interface IConsoleColorConverter
    {
        bool TryConvert(Color color, out ConsoleColor consoleColor);
    }
}
