using System;
using VoidMain.CommandLineIinterface.Internal;

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

        public void Handle(ConsoleInputEventArgs args)
        {
            if (args.IsHandledHint) return;

            switch (args.Input.Key)
            {
                case ConsoleKey.Backspace:
                    DeleteBackward(args);
                    break;
                case ConsoleKey.Delete:
                    DeleteForward(args);
                    break;
                case ConsoleKey.Escape:
                    ClearAll(args);
                    break;
                default:
                    break;
            }
        }

        private void DeleteBackward(ConsoleInputEventArgs args)
        {
            bool fast = args.Input.HasControlKey();
            var lineView = args.LineView;

            if (fast && lineView.ViewType == CommandLineViewType.Normal)
            {
                lineView.Delete(_fastNavigation.FindPrev(lineView) - lineView.Position);
                args.IsHandledHint = true;
            }
            else if (lineView.Position > 0)
            {
                lineView.Delete(-1);
                args.IsHandledHint = true;
            }
        }

        private void DeleteForward(ConsoleInputEventArgs args)
        {
            bool fast = args.Input.HasControlKey();
            var lineView = args.LineView;

            if (fast && lineView.ViewType == CommandLineViewType.Normal)
            {
                lineView.Delete(_fastNavigation.FindNext(lineView) - lineView.Position);
                args.IsHandledHint = true;
            }
            else if (lineView.Position < lineView.Length)
            {
                lineView.Delete(1);
                args.IsHandledHint = true;
            }
        }

        private void ClearAll(ConsoleInputEventArgs args)
        {
            args.LineView.ClearAll();
            args.IsHandledHint = true;
        }
    }
}
