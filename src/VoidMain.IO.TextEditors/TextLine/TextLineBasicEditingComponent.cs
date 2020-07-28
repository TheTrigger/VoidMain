using VoidMain.Text;

namespace VoidMain.IO.TextEditors.TextLine
{
    public class TextLineBasicEditingComponent<TText> : ITextEditorComponent<TText>
        where TText : class, ITextLine
    {
        private readonly ITextPositionSeeker? _textSeeker;
        private readonly TextLineBasicEditingComponentOptions _options;
        private bool _isInsertMode;

        public TextLineBasicEditingComponent(
            ITextPositionSeeker? textSeeker = null,
            TextLineBasicEditingComponentOptions? options = null)
        {
            _isInsertMode = false;
            _textSeeker = textSeeker;
            _options = options ?? new TextLineBasicEditingComponentOptions();
        }

        public void RegisterHandlers(ITextEditor<TText> editor)
        {
            editor.BindToAnyKey(TypeCharacter);
            editor.BindToKey(_options.ToggleInsertMode, ToggleInsertMode);

            editor.BindToKey(_options.DeleteBackward, DeleteBackward);
            editor.BindToKey(_options.DeleteForward, DeleteForward);

            editor.BindToKey(_options.DeleteBackwardFast, DeleteBackwardFast);
            editor.BindToKey(_options.DeleteForwardFast, DeleteForwardFast);

            editor.BindToKey(_options.MoveCursorToStart, MoveCursorToStart);
            editor.BindToKey(_options.MoveCursorToEnd, MoveCursorToEnd);

            editor.BindToKey(_options.MoveCursorBackward, MoveCursorBackward);
            editor.BindToKey(_options.MoveCursorForward, MoveCursorForward);

            editor.BindToKey(_options.MoveCursorBackwardFast, MoveCursorBackwardFast);
            editor.BindToKey(_options.MoveCursorForwardFast, MoveCursorForwardFast);

            editor.BindToKey(_options.ClearLine, ClearLine);
            editor.BindToKey(_options.CloseEditor, CloseEditor);
        }

        public void UnregisterHandlers(ITextEditor<TText> editor)
        {
            editor.UnbindFromAnyKey(TypeCharacter);
            editor.UnbindFromKey(_options.ToggleInsertMode, ToggleInsertMode);

            editor.UnbindFromKey(_options.DeleteBackward, DeleteBackward);
            editor.UnbindFromKey(_options.DeleteForward, DeleteForward);

            editor.UnbindFromKey(_options.DeleteBackwardFast, DeleteBackwardFast);
            editor.UnbindFromKey(_options.DeleteForwardFast, DeleteForwardFast);

            editor.UnbindFromKey(_options.MoveCursorToStart, MoveCursorToStart);
            editor.UnbindFromKey(_options.MoveCursorToEnd, MoveCursorToEnd);

            editor.UnbindFromKey(_options.MoveCursorBackward, MoveCursorBackward);
            editor.UnbindFromKey(_options.MoveCursorForward, MoveCursorForward);

            editor.UnbindFromKey(_options.MoveCursorBackwardFast, MoveCursorBackwardFast);
            editor.UnbindFromKey(_options.MoveCursorForwardFast, MoveCursorForwardFast);

            editor.UnbindFromKey(_options.ClearLine, ClearLine);
            editor.UnbindFromKey(_options.CloseEditor, CloseEditor);
        }

        private void TypeCharacter(IEditingContext<TText> context)
        {
            if (context.IsKeyBound) return;

            char c = context.Input.Character;
            if (char.IsControl(c)) return;

            if (_isInsertMode)
            {
                context.Text.TypeOver(c);
            }
            else
            {
                context.Text.Type(c);
            }
        }

        private void ToggleInsertMode(IEditingContext<TText> context)
        {
            _isInsertMode = !_isInsertMode;
        }

        private void DeleteBackward(IEditingContext<TText> context)
        {
            var text = context.Text;
            if (text.Position > 0)
            {
                text.Delete(-1);
            }
        }

        private void DeleteForward(IEditingContext<TText> context)
        {
            var text = context.Text;
            if (text.Position < text.Length)
            {
                text.Delete(1);
            }
        }

        private void DeleteBackwardFast(IEditingContext<TText> context)
        {
            if (_textSeeker == null) return;

            var text = context.Text;
            if (text.Position == 0) return;

            if (_textSeeker.TrySeekBackward(text.Span, text.Position, out int pos))
            {
                text.Delete(pos - text.Position);
            }
        }

        private void DeleteForwardFast(IEditingContext<TText> context)
        {
            if (_textSeeker == null) return;

            var text = context.Text;
            if (text.Position == text.Length) return;

            if (_textSeeker.TrySeekForward(text.Span, text.Position, out int pos))
            {
                text.Delete(pos - text.Position);
            }
        }

        private void MoveCursorToStart(IEditingContext<TText> context)
        {
            context.Text.Position = 0;
        }

        private void MoveCursorToEnd(IEditingContext<TText> context)
        {
            context.Text.Position = context.Text.Length;
        }

        private void MoveCursorBackward(IEditingContext<TText> context)
        {
            var text = context.Text;
            if (text.Position > 0)
            {
                text.Position--;
            }
        }

        private void MoveCursorForward(IEditingContext<TText> context)
        {
            var text = context.Text;
            if (text.Position < text.Length)
            {
                text.Position++;
            }
        }

        private void MoveCursorBackwardFast(IEditingContext<TText> context)
        {
            if (_textSeeker == null) return;

            var text = context.Text;
            if (text.Position == 0) return;

            if (_textSeeker.TrySeekBackward(text.Span, text.Position, out int pos))
            {
                text.Position = pos;
            }
        }

        private void MoveCursorForwardFast(IEditingContext<TText> context)
        {
            if (_textSeeker == null) return;

            var text = context.Text;
            if (text.Position == text.Length) return;

            if (_textSeeker.TrySeekForward(text.Span, text.Position, out int pos))
            {
                text.Position = pos;
            }
        }

        private void ClearLine(IEditingContext<TText> context)
        {
            context.Text.Clear();
        }

        private void CloseEditor(IEditingContext<TText> context)
        {
            context.CloseEditor();
        }
    }
}
