using System;
using VoidMain.CommandLineIinterface.Internal;

namespace VoidMain.CommandLineIinterface.Console.InputHandlers
{
    public class MoveCursorInputHandler : IConsoleInputHandler
    {
        private readonly ICommandLineFastNavigation _fastNavigation;

        public int Order { get; set; } = 2048;

        public MoveCursorInputHandler(ICommandLineFastNavigation fastNavigation)
        {
            _fastNavigation = fastNavigation ?? throw new ArgumentNullException(nameof(fastNavigation));
        }

        public bool Handle(ConsoleKeyInfo keyInfo, ICommandLineView lineView)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Home:
                    MoveToStart(lineView);
                    break;
                case ConsoleKey.End:
                    MoveToEnd(lineView);
                    break;
                case ConsoleKey.LeftArrow:
                    MoveLeft(lineView, keyInfo.HasControlKey());
                    break;
                case ConsoleKey.RightArrow:
                    MoveRight(lineView, keyInfo.HasControlKey());
                    break;
                default:
                    return false;
            }
            return true;
        }

        private void MoveToStart(ICommandLineView lineView)
        {
            lineView.MoveTo(0);
        }

        private void MoveToEnd(ICommandLineView lineView)
        {
            lineView.MoveTo(lineView.Length);
        }

        private void MoveLeft(ICommandLineView lineView, bool fast)
        {
            if (fast)
            {
                lineView.MoveTo(_fastNavigation.FindPrev(lineView));
            }
            else if (lineView.Position > 0)
            {
                lineView.Move(-1);
            }
        }

        private void MoveRight(ICommandLineView lineView, bool fast)
        {
            if (fast)
            {
                lineView.MoveTo(_fastNavigation.FindNext(lineView));
            }
            else if (lineView.Position < lineView.Length)
            {
                lineView.Move(1);
            }
        }
    }
}
