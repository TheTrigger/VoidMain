using System;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public interface IConsoleKeyConverter
    {
        InputKey ConvertKey(ConsoleKey key);
        InputModifiers ConvertModifiers(ConsoleModifiers modifiers);
    }
}
