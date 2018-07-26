using System;
using VoidMain.CommandLineInterface.IO;
using VoidMain.CommandLineInterface.IO.InputHandlers;
using VoidMain.CommandLineInterface.IO.Views;
using VoidMain.Hosting.Environment;

namespace VoidMain.CommandLineInterface.UndoRedo
{
    public class UndoRedoInputHandler : IInputHandler
    {
        private readonly IUndoRedoManager _undoRedoManager;
        private readonly IClock _clock;
        private readonly TimeSpan _minSnapshotTime;
        private DateTime _lastSnapshotTime;

        // We need to capture command line state before any modifications.
        public int Order => Int32.MinValue;

        public UndoRedoInputHandler(IUndoRedoManager undoRedoManager, IClock clock)
        {
            _undoRedoManager = undoRedoManager ?? throw new ArgumentNullException(nameof(undoRedoManager));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
            // Time to accumulate changes
            _minSnapshotTime = TimeSpan.FromSeconds(1.5);
            _lastSnapshotTime = DateTime.MinValue;
        }

        public void Handle(InputEventArgs args)
        {
            switch (args.Input.Key)
            {
                case InputKey.Z:
                    if (args.Input.HasControlKey())
                    {
                        Undo(args);
                    }
                    else
                    {
                        AddSnapshot(args.LineView);
                    }
                    break;
                case InputKey.Y:
                    if (args.Input.HasControlKey())
                    {
                        Redo(args);
                    }
                    else
                    {
                        AddSnapshot(args.LineView);
                    }
                    break;
                case InputKey.Enter:
                    ClearSnapshots();
                    break;
                default:
                    AddSnapshot(args.LineView);
                    break;
            }
        }

        private void Undo(InputEventArgs args)
        {
            var lineView = args.LineView;
            var currentSnapshot = lineView.TakeSnapshot();
            if (_undoRedoManager.TryUndo(currentSnapshot, out LineViewSnapshot prevSnapshot))
            {
                prevSnapshot.ApplyTo(lineView);
                args.IsHandledHint = true;
            }
        }

        private void Redo(InputEventArgs args)
        {
            var lineView = args.LineView;
            var currentSnapshot = lineView.TakeSnapshot();
            if (_undoRedoManager.TryRedo(currentSnapshot, out LineViewSnapshot nextSnapshot))
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

        private void AddSnapshot(ILineView lineView)
        {
            var now = _clock.UtcNow();
            if (now - _lastSnapshotTime < _minSnapshotTime)
            {
                return; // Too soon.
            }
            var snapshot = lineView.TakeSnapshot();
            if (_undoRedoManager.TryAddSnapshot(snapshot))
            {
                _lastSnapshotTime = now;
            }
        }
    }
}
