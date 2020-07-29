using System;
using VoidMain.IO.Keyboard;
using VoidMain.IO.TextEditors.Internal;

namespace VoidMain.IO.TextEditors.TextLine.History
{
    public class TextLineHistoryComponent<TText> : ITextEditorComponent<TText>, IDisposable
        where TText : class, ITextLine
    {
        private readonly IHistoryStorage? _storage;
        private readonly TextLineHistoryComponentOptions _options;
        private readonly PushOutCollection<string> _history;
        private readonly KeyHandler<TText> _modal;
        private TextLineSnapshot? _unsaved;
        private int _index;

        public TextLineHistoryComponent(
            IHistoryStorage? storage = null,
            TextLineHistoryComponentOptions? options = null)
        {
            options ??= new TextLineHistoryComponentOptions();

            if (options.Capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Capacity));
            }

            _options = options;
            _storage = storage;
            _history = new PushOutCollection<string>(_options.Capacity);
            _modal = Modal;
            _unsaved = null;
            _index = 0;

            var lines = _storage?.Load();
            if (lines == null) return;

            foreach (var line in lines)
            {
                _history.Add(line);
            }
            _index = _history.Count;
        }

        public void Dispose()
        {
            if (_storage == null) return;
            _storage.Save(_history);
        }

        public void RegisterHandlers(ITextEditor<TText> editor)
        {
            if (_options.Prev != KeyInfo.NoKey || _options.Next != KeyInfo.NoKey)
            {
                editor.BindToKey(_options.Prev, Prev);
                editor.BindToKey(_options.Next, Next);
                editor.StopEditing += Add;
            }
        }

        public void UnregisterHandlers(ITextEditor<TText> editor)
        {
            if (_options.Prev != KeyInfo.NoKey || _options.Next != KeyInfo.NoKey)
            {
                editor.UnbindFromKey(_options.Prev, Prev);
                editor.UnbindFromKey(_options.Next, Next);
                editor.StopEditing -= Add;
            }
        }

        private void Prev(IEditingContext<TText> context)
        {
            PrevTextLine(context.Text);
            context.EnterModalKeyHandling(_modal);
        }

        private void Next(IEditingContext<TText> context)
        {
            NextTextLine(context.Text);
            context.EnterModalKeyHandling(_modal);
        }

        private void Modal(IEditingContext<TText> context)
        {
            var key = context.Input.KeyInfo;

            if (key == _options.Prev)
            {
                PrevTextLine(context.Text);
            }
            else if (key == _options.Next)
            {
                NextTextLine(context.Text);
            }
            else
            {
                context.ExitModalKeyHandling(isKeyHandled: false);
            }
        }

        private void Add(TText text)
        {
            _index = _history.Count;
            _unsaved = null;

            var span = text.Span;
            if (span.IsWhiteSpace()) return;

            if (_history.Count > 0)
            {
                var last = _history[_history.Count - 1];
                if (span.SequenceEqual(last)) return;
            }

            _history.Add(new string(span));
            _index = _history.Count;
        }

        private void PrevTextLine(TText text)
        {
            if (_index == 0) return;

            if (_unsaved is null)
            {
                _unsaved = new TextLineSnapshot(text);
            }

            var prev = _history[--_index];
            text.Position = 0;
            text.TypeOver(prev);
            text.Delete(text.Length - text.Position);
        }

        private void NextTextLine(TText text)
        {
            if (_index + 1 < _history.Count)
            {
                var next = _history[++_index];
                text.Position = 0;
                text.TypeOver(next);
                text.Delete(text.Length - text.Position);
            }
            else if (_unsaved is TextLineSnapshot snapshot)
            {
                _index++;
                snapshot.ApplyTo(text);
                _unsaved = null;
            }
        }
    }
}
