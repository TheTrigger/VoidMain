﻿using System;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO.InputHandlers
{
    public class DeleteCharacterInputHandler : IConsoleInputHandler
    {
        private readonly ICommandLineViewNavigation _fastNavigation;

        public int Order { get; set; } = 1024;

        public DeleteCharacterInputHandler(ICommandLineViewNavigation fastNavigation)
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
                    Clear(args);
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
                lineView.Delete(_fastNavigation.FindPrevPosition(lineView) - lineView.Position);
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
                lineView.Delete(_fastNavigation.FindNextPosition(lineView) - lineView.Position);
                args.IsHandledHint = true;
            }
            else if (lineView.Position < lineView.Length)
            {
                lineView.Delete(1);
                args.IsHandledHint = true;
            }
        }

        private void Clear(ConsoleInputEventArgs args)
        {
            args.LineView.Clear();
            args.IsHandledHint = true;
        }
    }
}
