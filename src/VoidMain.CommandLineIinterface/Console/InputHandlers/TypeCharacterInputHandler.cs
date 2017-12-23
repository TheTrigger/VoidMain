using System;
using VoidMain.CommandLineIinterface.Internal;

namespace VoidMain.CommandLineIinterface.Console.InputHandlers
{
    public class TypeCharacterInputHandler : IConsoleInputHandler
    {
        private bool _insert = false;

        public int Order { get; set; } = 0;

        public bool Handle(ConsoleKeyInfo keyInfo, ICommandLineView lineView)
        {
            if (!Char.IsControl(keyInfo.KeyChar))
            {
                if (_insert)
                {
                    lineView.TypeOver(keyInfo.KeyChar);
                }
                else
                {
                    lineView.Type(keyInfo.KeyChar);
                }
                return true;
            }
            else if (keyInfo.Key == ConsoleKey.Insert && keyInfo.HasNoModifiers())
            {
                _insert = !_insert;
                return true;
            }
            return false;
        }
    }
}
