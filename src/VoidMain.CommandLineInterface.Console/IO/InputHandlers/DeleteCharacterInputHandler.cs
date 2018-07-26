using System;
using VoidMain.CommandLineInterface.IO.Console;
using VoidMain.CommandLineInterface.IO.Views;

namespace VoidMain.CommandLineInterface.IO.InputHandlers
{
    public class DeleteCharacterInputHandler : IInputHandler
    {
        private readonly ILineViewNavigation _lineViewNavigation;

        public int Order { get; set; } = 1024;

        public DeleteCharacterInputHandler(ILineViewNavigation lineViewNavigation)
        {
            _lineViewNavigation = lineViewNavigation ?? throw new ArgumentNullException(nameof(lineViewNavigation));
        }

        public void Handle(InputEventArgs args)
        {
            if (args.IsHandledHint) return;

            switch (args.Input.Key)
            {
                case InputKey.Backspace:
                    DeleteBackward(args);
                    break;
                case InputKey.Delete:
                    DeleteForward(args);
                    break;
                case InputKey.Escape:
                    Clear(args);
                    break;
                default:
                    break;
            }
        }

        private void DeleteBackward(InputEventArgs args)
        {
            bool fast = args.Input.HasControlKey();
            var lineView = args.LineView;
            args.IsHandledHint = true;

            if (lineView.Position == 0) return;

            if (fast && lineView.ViewType == LineViewType.Normal)
            {
                int prev = _lineViewNavigation.FindPrevPosition(lineView);
                lineView.Delete(prev - lineView.Position);
            }
            else
            {
                lineView.Delete(-1);
            }
        }

        private void DeleteForward(InputEventArgs args)
        {
            bool fast = args.Input.HasControlKey();
            var lineView = args.LineView;
            args.IsHandledHint = true;

            if (lineView.Position == lineView.Length) return;

            if (fast && lineView.ViewType == LineViewType.Normal)
            {
                int next = _lineViewNavigation.FindNextPosition(lineView);
                lineView.Delete(next - lineView.Position);
            }
            else
            {
                lineView.Delete(1);
            }
        }

        private void Clear(InputEventArgs args)
        {
            args.LineView.Clear();
            args.IsHandledHint = true;
        }
    }
}
