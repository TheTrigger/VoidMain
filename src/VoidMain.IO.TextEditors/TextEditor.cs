using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.IO.Keyboard;

namespace VoidMain.IO.TextEditors
{
    public class TextEditor<TText> : ITextEditor<TText>
        where TText : class
    {
        private readonly IKeyReader _keyReader;
        private readonly Context _context;
        private readonly Dictionary<KeyInfo, KeyHandler<TText>> _keyBindings;
        private KeyHandler<TText>? _anyKeyBindings;

        public event EditingPhase<TText>? StartEditing;
        public event EditingPhase<TText>? StopEditing;

        public TextEditor(IKeyReader keyReader)
        {
            _keyReader = keyReader ?? throw new ArgumentNullException(nameof(keyReader));
            _context = new Context();
            _keyBindings = new Dictionary<KeyInfo, KeyHandler<TText>>();
        }

        public bool IsKeyBound(KeyInfo key)
        {
            return key == KeyInfo.AnyKey
                ? _anyKeyBindings != null
                : _keyBindings.ContainsKey(key);
        }

        public void BindToAnyKey(KeyHandler<TText> handler)
            => _anyKeyBindings += handler;

        public void UnbindFromAnyKey(KeyHandler<TText> handler)
            => _anyKeyBindings -= handler;

        public void UnbindFromAnyKey() => _anyKeyBindings = null;

        public void BindToKey(KeyInfo key, KeyHandler<TText> handler)
        {
            if (handler == null) return;
            if (key == KeyInfo.NoKey) return;

            if (key == KeyInfo.AnyKey)
            {
                BindToAnyKey(handler);
                return;
            }

            _keyBindings[key] = _keyBindings.GetValueOrDefault(key) + handler;
        }

        public void UnbindFromKey(KeyInfo key, KeyHandler<TText> handler)
        {
            if (key == KeyInfo.AnyKey)
            {
                UnbindFromAnyKey(handler);
                return;
            }

            if (_keyBindings.TryGetValue(key, out var handlersForKey))
            {
                handlersForKey -= handler;
                if (handlersForKey == null)
                {
                    _keyBindings.Remove(key);
                }
                else
                {
                    _keyBindings[key] = handlersForKey;
                }
            }
        }

        public void UnbindFromKey(KeyInfo key)
        {
            if (key == KeyInfo.AnyKey)
            {
                UnbindFromAnyKey();
                return;
            }

            _keyBindings.Remove(key);
        }

        public void UnbindFromAllKeys()
        {
            _keyBindings.Clear();
            _anyKeyBindings = null;
        }

        public async Task EditAsync(
            TText text,
            IEditingEventsListener? eventsListener = null,
            CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();

            StartEditing?.Invoke(text);

            try
            {
                _context.Text = text;
                _context.Cancellation = token;

                while (!token.IsCancellationRequested)
                {
                    if (_context.ModalKeyHandler == null)
                    {
                        _context.Input = await _keyReader.ReadKeyAsync(token).ConfigureAwait(false);
                        unchecked { _context.InputId++; }

                        eventsListener?.OnModifying(_context.Input.IsNextKeyAvailable);
                    }
                    else
                    {
                        await EditModalAsync(eventsListener, token).ConfigureAwait(false);
                        if (_context.IsCloseRequested) break;
                    }

                    if (_keyBindings.TryGetValue(_context.Input.KeyInfo, out var handlers))
                    {
                        _context.IsKeyBound = true;
                        handlers.Invoke(_context);
                    }
                    else
                    {
                        _context.IsKeyBound = false;
                    }

                    _anyKeyBindings?.Invoke(_context);

                    if (_context.IsCloseRequested)
                    {
                        eventsListener?.OnModified(isNextChangeAvailable: false);
                        break;
                    }

                    eventsListener?.OnModified(_context.Input.IsNextKeyAvailable);
                }
            }
            finally
            {
                _context.Reset();
                StopEditing?.Invoke(text);
            }

            token.ThrowIfCancellationRequested();
        }

        private async Task EditModalAsync(IEditingEventsListener? eventsListener, CancellationToken token)
        {
            _context.IsModalKeyHandlingInProgress = true;
            _context.IsKeyBound = false;

            while (!token.IsCancellationRequested)
            {
                _context.Input = await _keyReader.ReadKeyAsync(token).ConfigureAwait(false);
                unchecked { _context.InputId++; }

                eventsListener?.OnModifying(_context.Input.IsNextKeyAvailable);

                _context.ModalKeyHandler(_context);

                if (_context.IsCloseRequested)
                {
                    eventsListener?.OnModified(isNextChangeAvailable: false);
                    return;
                }

                if (!_context.IsModalKeyHandlingInProgress)
                {
                    if (_context.IsModalKeyHandled)
                    {
                        eventsListener?.OnModified(_context.Input.IsNextKeyAvailable);

                        _context.Input = await _keyReader.ReadKeyAsync(token).ConfigureAwait(false);
                        unchecked { _context.InputId++; }

                        eventsListener?.OnModifying(_context.Input.IsNextKeyAvailable);
                    }

                    return;
                }

                eventsListener?.OnModified(_context.Input.IsNextKeyAvailable);
            }
        }

        private sealed class Context : IEditingContext<TText>
        {
            public TText Text { get; set; } = null!;
            public CancellationToken Cancellation { get; set; }

            public KeyInput Input { get; set; }
            public uint InputId { get; set; }
            public bool IsKeyBound { get; set; }

            public KeyHandler<TText> ModalKeyHandler { get; private set; } = null!;
            public bool IsModalKeyHandlingEnabled => ModalKeyHandler != null;
            public bool IsModalKeyHandlingInProgress { get; set; }
            public bool IsModalKeyHandled { get; private set; }

            public void EnterModalKeyHandling(KeyHandler<TText> handler)
            {
                if (ModalKeyHandler != null)
                {
                    throw new Exception("Modal key handling is already requested.");
                }
                ModalKeyHandler = handler;
            }

            public void ExitModalKeyHandling(bool isKeyHandled)
            {
                if (!IsModalKeyHandlingInProgress)
                {
                    throw new Exception("Can't exit modal key handling because it is not active.");
                }
                ModalKeyHandler = null!;
                IsModalKeyHandled = isKeyHandled;
                IsModalKeyHandlingInProgress = false;
            }

            public bool IsCloseRequested { get; private set; }
            public void CloseEditor() => IsCloseRequested = true;

            public void Reset()
            {
                Text = null!;
                Cancellation = default;
                InputId = 0;
                IsKeyBound = false;
                ModalKeyHandler = null!;
                IsModalKeyHandlingInProgress = false;
                IsModalKeyHandled = false;
                IsCloseRequested = false;
            }
        }
    }
}
