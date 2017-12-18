using System;

namespace VoidMain.CommandLineIinterface.Console.InputHandlers
{
    public class TypeCharacterInputHandler : IConsoleInputHandler
    {
        public int Order { get; set; } = 0;

        public bool Handle(ConsoleKeyInfo keyInfo, ICommandLineView lineView)
        {
            // TODO: Handle 'Insert' key
            if (!Char.IsControl(keyInfo.KeyChar))
            {
                lineView.Type(keyInfo.KeyChar);
                return true;
            }
            return false;
        }
    }
}
