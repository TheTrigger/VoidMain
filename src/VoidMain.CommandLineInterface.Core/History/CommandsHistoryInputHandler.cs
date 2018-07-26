using System;
using VoidMain.CommandLineInterface.IO;
using VoidMain.CommandLineInterface.IO.InputHandlers;
using VoidMain.CommandLineInterface.IO.Views;

namespace VoidMain.CommandLineInterface.History
{
    public class CommandsHistoryInputHandler : IInputHandler
    {
        private readonly ICommandsHistoryManager _historyManager;
        private LineViewSnapshot? _temp;

        public int Order { get; set; } = 2048;

        public CommandsHistoryInputHandler(ICommandsHistoryManager historyManager)
        {
            _historyManager = historyManager ?? throw new ArgumentNullException(nameof(historyManager));
            _temp = null;
        }

        public void Handle(InputEventArgs args)
        {
            if (args.IsHandledHint) return;

            switch (args.Input.Key)
            {
                case InputKey.UpArrow:
                    SetPrevCommand(args);
                    break;
                case InputKey.DownArrow:
                    SetNextCommand(args);
                    break;
                case InputKey.Enter:
                    AddCommand(args);
                    break;
                default:
                    break;
            }
        }

        private void SetPrevCommand(InputEventArgs args)
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

        private void SetNextCommand(InputEventArgs args)
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

        private void AddCommand(InputEventArgs args)
        {
            string command = args.LineView.ToString();
            _historyManager.AddCommand(command);
            _temp = null;
            args.IsHandledHint = true;
        }
    }
}
