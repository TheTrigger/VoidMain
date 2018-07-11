using System;
using VoidMain.CommandLineIinterface.IO.InputHandlers;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryInputHandler : IConsoleInputHandler
    {
        private readonly ICommandsHistoryManager _historyManager;
        private LineViewSnapshot? _temp;

        public int Order { get; set; } = 2048;

        public CommandsHistoryInputHandler(ICommandsHistoryManager historyManager)
        {
            _historyManager = historyManager ?? throw new ArgumentNullException(nameof(historyManager));
            _temp = null;
        }

        public void Handle(ConsoleInputEventArgs args)
        {
            if (args.IsHandledHint) return;

            switch (args.Input.Key)
            {
                case ConsoleKey.UpArrow:
                    SetPrevCommand(args);
                    break;
                case ConsoleKey.DownArrow:
                    SetNextCommand(args);
                    break;
                case ConsoleKey.Enter:
                    AddCommand(args);
                    break;
                default:
                    break;
            }
        }

        private void SetPrevCommand(ConsoleInputEventArgs args)
        {
            if (_historyManager.TryGetPrevCommand(out string command))
            {
                if (!_temp.HasValue)
                {
                    _temp = args.LineView.TakeSnapshot();
                }
                args.LineView.ReplaceWith(command);
                args.IsHandledHint = true;
            }
        }

        private void SetNextCommand(ConsoleInputEventArgs args)
        {
            if (_historyManager.TryGetNextCommand(out string command))
            {
                args.LineView.ReplaceWith(command);
                args.IsHandledHint = true;
            }
            else if (_temp.HasValue)
            {
                _temp.Value.ApplyTo(args.LineView);
                _temp = null;
                args.IsHandledHint = true;
            }
        }

        private void AddCommand(ConsoleInputEventArgs args)
        {
            string command = args.LineView.ToString();
            _historyManager.AddCommand(command);
            _temp = null;
            args.IsHandledHint = true;
        }
    }
}
