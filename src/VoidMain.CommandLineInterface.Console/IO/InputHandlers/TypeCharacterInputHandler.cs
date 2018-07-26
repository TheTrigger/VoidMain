using System;
using VoidMain.CommandLineInterface.IO.Console;

namespace VoidMain.CommandLineInterface.IO.InputHandlers
{
    public class TypeCharacterInputHandler : IInputHandler
    {
        private bool _insert = false;

        public int Order { get; set; } = 0;

        public void Handle(InputEventArgs args)
        {
            if (args.IsHandledHint) return;

            var input = args.Input;
            var lineView = args.LineView;

            if (!Char.IsControl(input.Character))
            {
                if (_insert)
                {
                    lineView.TypeOver(input.Character);
                }
                else
                {
                    lineView.Type(input.Character);
                }
                args.IsHandledHint = true;
            }
            else if (input.Key == InputKey.Insert && input.HasNoModifiers())
            {
                _insert = !_insert;
                args.IsHandledHint = true;
            }
        }
    }
}
