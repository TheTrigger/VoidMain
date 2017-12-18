using System;

namespace VoidMain.CommandLineIinterface.Console
{
    public interface IConsoleInputHandler
    {
        int Order { get; }
        bool Handle(ConsoleKeyInfo keyInfo, ICommandLineView view);
    }
}
