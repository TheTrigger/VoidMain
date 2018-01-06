using System;
using VoidMain.CommandLineIinterface.History;
using VoidMain.CommandLineIinterface.IO.Views;

namespace VoidMain.CommandLineIinterface.IO.Console.InputHandlers
{
    public class CommandsHistoryInputHandler : IConsoleInputHandler
    {
        private readonly ICommandsHistoryManager _historyManager;
        private string _temp;

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
                if (_temp == null)
                {
                    _temp = args.LineView.ToString();
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
            else if (_temp != null)
            {
                args.LineView.ReplaceWith(_temp);
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
