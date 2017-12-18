using System;

namespace VoidMain.CommandLineIinterface.Console.InputHandlers
{
    public class DeleteCharacterInputHandler : IConsoleInputHandler
    {
        private readonly ICommandLineFastNavigation _fastNavigation;

        public int Order { get; set; } = 1024;

        public DeleteCharacterInputHandler(ICommandLineFastNavigation fastNavigation)
        {
            _fastNavigation = fastNavigation ?? throw new ArgumentNullException(nameof(fastNavigation));
        }

        public bool Handle(ConsoleKeyInfo keyInfo, ICommandLineView lineView)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    ClearAll(lineView);
                    break;
                case ConsoleKey.Backspace:
                    DeleteBackward(lineView, HasControlKey(keyInfo));
                    break;
                case ConsoleKey.Delete:
                    DeleteForward(lineView, HasControlKey(keyInfo));
                    break;
                default:
                    return false;
            }
            return true;
        }

        private void ClearAll(ICommandLineView lineView)
        {
            lineView.ClearAll();
        }

        private void DeleteBackward(ICommandLineView lineView, bool fast)
        {
            if (fast)
            {
                lineView.Delete(_fastNavigation.FindPrev(lineView) - lineView.Position);
            }
            else if (lineView.Position > 0)
            {
                lineView.Delete(-1);
            }
        }

        private void DeleteForward(ICommandLineView lineView, bool fast)
        {
            if (fast)
            {
                lineView.Delete(_fastNavigation.FindNext(lineView) - lineView.Position);
            }
            else if (lineView.Position < lineView.Length)
            {
                lineView.Delete(1);
            }
        }

        private bool HasControlKey(ConsoleKeyInfo keyInfo)
        {
            return keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control);
        }
    }
}
