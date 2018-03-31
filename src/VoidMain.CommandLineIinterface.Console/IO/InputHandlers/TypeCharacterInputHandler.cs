using System;
using VoidMain.CommandLineIinterface.IO.Internal;

namespace VoidMain.CommandLineIinterface.IO.InputHandlers
{
    public class TypeCharacterInputHandler : IConsoleInputHandler
    {
        private bool _insert = false;

        public int Order { get; set; } = 0;

        public void Handle(ConsoleInputEventArgs args)
        {
            if (args.IsHandledHint) return;

            var input = args.Input;
            var lineView = args.LineView;

            if (!Char.IsControl(input.KeyChar))
            {
                if (_insert)
                {
                    lineView.TypeOver(input.KeyChar);
                }
                else
                {
                    lineView.Type(input.KeyChar);
                }
                args.IsHandledHint = true;
            }
            else if (input.Key == ConsoleKey.Insert && input.HasNoModifiers())
            {
                _insert = !_insert;
                args.IsHandledHint = true;
            }
        }
    }
}
