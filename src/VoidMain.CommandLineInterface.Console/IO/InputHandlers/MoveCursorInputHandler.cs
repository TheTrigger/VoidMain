using System;
using VoidMain.CommandLineInterface.IO.Console;
using VoidMain.CommandLineInterface.IO.Views;

namespace VoidMain.CommandLineInterface.IO.InputHandlers
{
    public class MoveCursorInputHandler : IInputHandler
    {
        private readonly ILineViewNavigation _lineViewNavigation;

        public int Order { get; set; } = 2048;

        public MoveCursorInputHandler(ILineViewNavigation lineViewNavigation)
        {
            _lineViewNavigation = lineViewNavigation ?? throw new ArgumentNullException(nameof(lineViewNavigation));
        }

        public void Handle(InputEventArgs args)
        {
            if (args.IsHandledHint) return;

            switch (args.Input.Key)
            {
                case InputKey.Home:
                    MoveToStart(args);
                    break;
                case InputKey.End:
                    MoveToEnd(args);
                    break;
                case InputKey.LeftArrow:
                    MoveLeft(args);
                    break;
                case InputKey.RightArrow:
                    MoveRight(args);
                    break;
                default:
                    break;
            }
        }

        private void MoveToStart(InputEventArgs args)
        {
            args.LineView.MoveTo(0);
            args.IsHandledHint = true;
        }

        private void MoveToEnd(InputEventArgs args)
        {
            args.LineView.MoveTo(args.LineView.Length);
            args.IsHandledHint = true;
        }

        private void MoveLeft(InputEventArgs args)
        {
            bool fast = args.Input.HasControlKey();
            var lineView = args.LineView;
            args.IsHandledHint = true;

            if (lineView.Position == 0) return;

            if (fast && lineView.ViewType == LineViewType.Normal)
            {
                int prev = _lineViewNavigation.FindPrevPosition(lineView);
                lineView.MoveTo(prev);
            }
            else
            {
                lineView.Move(-1);
            }
        }

        private void MoveRight(InputEventArgs args)
        {
            bool fast = args.Input.HasControlKey();
            var lineView = args.LineView;
            args.IsHandledHint = true;

            if (lineView.Position == lineView.Length) return;

            if (fast && lineView.ViewType == LineViewType.Normal)
            {
                int next = _lineViewNavigation.FindNextPosition(lineView);
                lineView.MoveTo(next);
            }
            else
            {
                lineView.Move(1);
            }
        }
    }
}
