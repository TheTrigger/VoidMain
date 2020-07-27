using System;
using VoidMain.IO.Console;

namespace VoidMain.IO.TextEditors.TextLine
{
    public class ConsoleMaskedTextLineView : ITextLineView, IDisposable
    {
        private readonly IConsole _console;
        private readonly ConsoleContinuousCursor _cursor;
        private readonly ArrayPoolTextLine _line;
        private readonly char _mask;

        public int Length => _line.Length;

        public int Position
        {
            get => _line.Position;
            set
            {
                int oldPos = Position;
                _line.Position = value;
                _cursor.Move(value - oldPos);
            }
        }

        public ReadOnlySpan<char> Span => _line.Span;

        public uint ContentVersion => _line.ContentVersion;

        public ConsoleMaskedTextLineView(IConsole console, char mask)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = new ConsoleContinuousCursor(console);
            _line = new ArrayPoolTextLine(console.BufferWidth);
            _mask = mask;
        }

        ~ConsoleMaskedTextLineView() => Dispose();

        public void Dispose() => _line.Dispose();

        public override string ToString() => _line.ToString();

        public void Clear()
        {
            int width = _console.BufferWidth;
            int left = _console.CursorLeft + (Length - Position);
            int clear = Length;
            if (left % width == 0) clear++;

            _cursor.Move(-Position);
            _console.Write(' ', clear);
            _cursor.Move(-clear);
            _line.Clear();
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            _line.Delete(count);

            if (count < 0)
            {
                _cursor.Move(count);
                count = -count;
            }

            int diff = Length - Position;
            _cursor.Move(diff);

            int width = _console.BufferWidth;
            int left = _console.CursorLeft + count;
            if (left % width == 0) count++;

            _console.Write(' ', count);
            _cursor.Move(-diff - count);
        }

        public void Type(char value)
        {
            int diff = Length - Position;
            int width = _console.BufferWidth;
            int left = _console.CursorLeft + 1 + diff;

            _cursor.Move(diff);
            _console.Write(_mask);
            if (left % width == 0)
            {
                _console.Write(' ');
                diff++;
            }
            _cursor.Move(-diff);

            _line.Type(value);
        }

        public void Type(string value)
        {
            if (value == null) return;
            Type(value.AsSpan());
        }

        public void Type(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty) return;

            int diff = Length - Position;
            int width = _console.BufferWidth;
            int left = _console.CursorLeft + value.Length + diff;

            _cursor.Move(diff);
            _console.Write(_mask, value.Length);
            if (left % width == 0)
            {
                _console.Write(' ');
                diff++;
            }
            _cursor.Move(-diff);

            _line.Type(value);
        }

        public void TypeOver(char value)
        {
            int width = _console.BufferWidth;
            int left = _console.CursorLeft + 1;

            if (Position == Length)
            {
                _console.Write(_mask);
                if (left == width) FixCursor();
            }
            else
            {
                _cursor.Move(1);
            }

            _line.TypeOver(value);
        }

        public void TypeOver(string value)
        {
            if (value == null) return;
            TypeOver(value.AsSpan());
        }

        public void TypeOver(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty) return;

            int width = _console.BufferWidth;
            int left = _console.CursorLeft + value.Length;

            if (Position + value.Length > Length)
            {
                _console.Write(_mask, value.Length);
                if (left % width == 0) FixCursor();
            }
            else
            {
                _cursor.Move(value.Length);
            }

            _line.TypeOver(value);
        }

        private void FixCursor()
        {
            _console.Write(' ');
            _console.CursorLeft = 0;
        }

        public void ClearState()
        {
            _line.ClearState();
        }

        public void SetState(ReadOnlySpan<char> text, int position)
        {
            _line.SetState(text, position);
        }

        public void Render()
        {
            _cursor.Move(-Position);

            int width = _console.BufferWidth;
            int left = _console.CursorLeft + Length;

            _console.Write(_mask, Length);

            if (left % width == 0) FixCursor();
            _cursor.Move(Position - Length);
        }

        public void EndLine()
        {
            Position = Length;
            ClearState();
            if (_console.CursorLeft > 0)
            {
                _console.WriteLine();
            }
        }
    }
}
