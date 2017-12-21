using System;
using VoidMain.CommandLineIinterface.History;

namespace VoidMain.CommandLineIinterface.Console.InputHandlers
{
    class CommandsHistoryInputHandler : IConsoleInputHandler
    {
        private readonly ICommandsHistoryManager _historyManager;
        private string _temp;

        public int Order { get; set; } = 2048;

        public CommandsHistoryInputHandler(ICommandsHistoryManager historyManager)
        {
            _historyManager = historyManager ?? throw new ArgumentNullException(nameof(historyManager));
            _temp = null;
        }

        public bool Handle(ConsoleKeyInfo keyInfo, ICommandLineView view)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    SetPrevCommand(view);
                    break;
                case ConsoleKey.DownArrow:
                    SetNextCommand(view);
                    break;
                case ConsoleKey.Enter:
                    AddCommand(view);
                    break;
                default:
                    return false;
            }
            return true;
        }

        private void SetPrevCommand(ICommandLineView view)
        {
            if (_historyManager.TryGetPrevCommand(out string command))
            {
                if (_temp == null)
                {
                    _temp = view.ToString();
                }
                view.ReplaceWith(command);
            }
        }

        private void SetNextCommand(ICommandLineView view)
        {
            if (_historyManager.TryGetNextCommand(out string command))
            {
                view.ReplaceWith(command);
            }
            else if (_temp != null)
            {
                view.ReplaceWith(_temp);
                _temp = null;
            }
        }

        private void AddCommand(ICommandLineView view)
        {
            string command = view.ToString();
            _historyManager.AddCommand(command);
            _temp = null;
        }
    }
}
