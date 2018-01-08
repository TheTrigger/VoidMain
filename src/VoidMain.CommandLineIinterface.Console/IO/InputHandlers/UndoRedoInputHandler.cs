using System;
using VoidMain.CommandLineIinterface.IO.Console.Internal;
using VoidMain.CommandLineIinterface.IO.Views;
using VoidMain.CommandLineIinterface.UndoRedo;

namespace VoidMain.CommandLineIinterface.IO.Console.InputHandlers
{
    public class UndoRedoInputHandler : IConsoleInputHandler
    {
        private readonly IUndoRedoManager<CommandLineViewSnapshot> _undoRedoManager;
        private readonly TimeSpan _minSnapshotTime;
        private DateTime _lastSnapshotTime;

        // We need to capture command line state before any modifications.
        public int Order => Int32.MinValue;

        public UndoRedoInputHandler(IUndoRedoManager<CommandLineViewSnapshot> undoRedoManager)
        {
            _undoRedoManager = undoRedoManager ?? throw new ArgumentNullException(nameof(undoRedoManager));
            // Time to accumulate changes
            _minSnapshotTime = TimeSpan.FromSeconds(1.5);
            _lastSnapshotTime = DateTime.MinValue;
        }

        public void Handle(ConsoleInputEventArgs args)
        {
            switch (args.Input.Key)
            {
                case ConsoleKey.Z:
                    if (args.Input.HasControlKey())
                    {
                        Undo(args);
                    }
                    else
                    {
                        AddSnapshot(args.LineView, byTime: true);
                    }
                    break;
                case ConsoleKey.Y:
                    if (args.Input.HasControlKey())
                    {
                        Redo(args);
                    }
                    else
                    {
                        AddSnapshot(args.LineView, byTime: true);
                    }
                    break;
                case ConsoleKey.Enter:
                    ClearSnapshots();
                    break;
                default:
                    AddSnapshot(args.LineView, byTime: true);
                    break;
            }
        }

        private void Undo(ConsoleInputEventArgs args)
        {
            var lineView = args.LineView;
            var currentSnapshot = lineView.TakeSnapshot();
            if (_undoRedoManager.TryUndo(currentSnapshot, out CommandLineViewSnapshot prevSnapshot))
            {
                prevSnapshot.ApplyTo(lineView);
                args.IsHandledHint = true;
            }
        }

        private void Redo(ConsoleInputEventArgs args)
        {
            var lineView = args.LineView;
            var currentSnapshot = lineView.TakeSnapshot();
            if (_undoRedoManager.TryRedo(currentSnapshot, out CommandLineViewSnapshot nextSnapshot))
            {
                nextSnapshot.ApplyTo(lineView);
                args.IsHandledHint = true;
            }
        }

        private void ClearSnapshots()
        {
            _undoRedoManager.Clear();
            _lastSnapshotTime = DateTime.MinValue;
        }

        private void AddSnapshot(ICommandLineView lineView, bool byTime = false)
        {
            var now = DateTime.UtcNow;
            if (byTime && now - _lastSnapshotTime < _minSnapshotTime)
            {
                // Too soon.
                return;
            }
            var snapshot = lineView.TakeSnapshot();
            if (_undoRedoManager.TryAddSnapshot(snapshot, deleteAfter: byTime))
            {
                _lastSnapshotTime = now;
            }
        }
    }
}
