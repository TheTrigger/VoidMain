﻿using System;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO.InputHandlers
{
    public class MoveCursorInputHandler : IConsoleInputHandler
    {
        private readonly ILineViewNavigation _lineViewNavigation;

        public int Order { get; set; } = 2048;

        public MoveCursorInputHandler(ILineViewNavigation lineViewNavigation)
        {
            _lineViewNavigation = lineViewNavigation ?? throw new ArgumentNullException(nameof(lineViewNavigation));
        }

        public void Handle(ConsoleInputEventArgs args)
        {
            if (args.IsHandledHint) return;

            switch (args.Input.Key)
            {
                case ConsoleKey.Home:
                    MoveToStart(args);
                    break;
                case ConsoleKey.End:
                    MoveToEnd(args);
                    break;
                case ConsoleKey.LeftArrow:
                    MoveLeft(args);
                    break;
                case ConsoleKey.RightArrow:
                    MoveRight(args);
                    break;
                default:
                    break;
            }
        }

        private void MoveToStart(ConsoleInputEventArgs args)
        {
            args.LineView.MoveTo(0);
            args.IsHandledHint = true;
        }

        private void MoveToEnd(ConsoleInputEventArgs args)
        {
            args.LineView.MoveTo(args.LineView.Length);
            args.IsHandledHint = true;
        }

        private void MoveLeft(ConsoleInputEventArgs args)
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

        private void MoveRight(ConsoleInputEventArgs args)
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