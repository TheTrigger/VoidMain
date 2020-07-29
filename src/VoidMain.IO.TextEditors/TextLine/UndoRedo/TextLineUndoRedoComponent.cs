using System;
using System.Linq;
using VoidMain.IO.Clock;
using VoidMain.IO.Keyboard;
using VoidMain.IO.TextEditors.Internal;

namespace VoidMain.IO.TextEditors.TextLine.UndoRedo
{
    public class TextLineUndoRedoComponent<TText> : ITextEditorComponent<TText>
        where TText : class, ITextLine
    {
        private readonly IClock? _clock;
        private readonly TextLineUndoRedoComponentOptions _options;
        private readonly PushOutCollection<TextLineSnapshot> _snapshots;
        private readonly KeyHandler<TText> _modal;
        private long _accumulationPeriod;
        private long _lastTimestamp;
        private int _index;

        public TextLineUndoRedoComponent(
            IClock? clock = null,
            TextLineUndoRedoComponentOptions? options = null)
        {
            options ??= new TextLineUndoRedoComponentOptions();

            if (options.MaxSteps < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(options.MaxSteps));
            }
            if (clock == null && options.AccumulationPeriod > TimeSpan.Zero)
            {
                throw new ArgumentNullException(nameof(clock),
                    "Clock must be initialized if accumulation period is more than zero");
            }

            _clock = clock;
            _options = options;
            _snapshots = new PushOutCollection<TextLineSnapshot>(_options.MaxSteps);
            _modal = Modal;
            _accumulationPeriod = _options.AccumulationPeriod.Ticks;
            _lastTimestamp = 0;
            _index = -1;
        }

        public void RegisterHandlers(ITextEditor<TText> editor)
        {
            if (_options.Undo != KeyInfo.NoKey || _options.Redo != KeyInfo.NoKey)
            {
                editor.BindToKey(_options.Undo, Undo);
                editor.BindToKey(_options.Redo, Redo);
                editor.BindToAnyKey(Add);
                editor.StartEditing += Initialize;
                editor.StopEditing += CleanUp;
            }
        }

        public void UnregisterHandlers(ITextEditor<TText> editor)
        {
            if (_options.Undo != KeyInfo.NoKey || _options.Redo != KeyInfo.NoKey)
            {
                editor.UnbindFromKey(_options.Undo, Undo);
                editor.UnbindFromKey(_options.Redo, Redo);
                editor.UnbindFromAnyKey(Add);
                editor.StartEditing -= Initialize;
                editor.StopEditing -= CleanUp;
            }
        }

        private void Undo(IEditingContext<TText> context)
        {
            UndoTextLine(context.Text);
            context.EnterModalKeyHandling(_modal);
        }

        private void Redo(IEditingContext<TText> context)
        {
            RedoTextLine(context.Text);
            context.EnterModalKeyHandling(_modal);
        }

        private void Modal(IEditingContext<TText> context)
        {
            var key = context.Input.KeyInfo;

            if (key == _options.Undo)
            {
                UndoTextLine(context.Text);
            }
            else if (key == _options.Redo)
            {
                RedoTextLine(context.Text);
            }
            else
            {
                context.ExitModalKeyHandling(isKeyHandled: false);
            }
        }

        private void Initialize(TText text)
        {
            var snapshot = new TextLineSnapshot(text);
            _snapshots.Add(snapshot);
            _index = 0;
            _lastTimestamp = _clock?.GetTimestamp() ?? 0;
        }

        private void CleanUp(TText text)
        {
            _snapshots.Clear();
            _index = -1;
        }

        private void Add(IEditingContext<TText> context)
        {
            if (context.Input.IsNextKeyAvailable) return;
            if (context.IsModalKeyHandlingEnabled) return;

            var text = context.Text;
            var current = _snapshots[_index];

            if (text.Span.SequenceEqual(current.Content))
            {
                _snapshots[_index] = current.WithNewPosition(text.Position);
                return;
            }

            if (_accumulationPeriod > 0)
            {
                long now = _clock!.GetTimestamp();
                if (now - _lastTimestamp < _accumulationPeriod) return;
                _lastTimestamp = now;
            }

            AddAfterCurrent(text);
        }

        private void UndoTextLine(TText text)
        {
            if (!text.Span.SequenceEqual(_snapshots[_index].Content))
            {
                AddAfterCurrent(text);
            }

            if (_index > 0)
            {
                var prev = _snapshots[--_index];
                prev.ApplyTo(text);
            }
        }

        private void RedoTextLine(TText text)
        {
            if (!text.Span.SequenceEqual(_snapshots[_index].Content))
            {
                AddAfterCurrent(text);
            }

            if (_index + 1 < _snapshots.Count)
            {
                var next = _snapshots[++_index];
                next.ApplyTo(text);
            }
        }

        private void AddAfterCurrent(TText text)
        {
            if (_index + 1 < _snapshots.Count)
            {
                _snapshots.TrimTo(_index + 1);
            }

            var snapshot = new TextLineSnapshot(text);
            _snapshots.Add(snapshot);
            _index = _snapshots.Count - 1;
        }
    }
}
