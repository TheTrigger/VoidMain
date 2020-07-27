using System;
using VoidMain.IO.Console;

namespace VoidMain.IO.TextEditors.TextLine
{
    public class ConsoleTextLineView : ITextLineView, IDisposable
    {
        private readonly IConsole _console;
        private readonly ConsoleContinuousCursor _cursor;
        private readonly ArrayPoolTextLine _line;

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

        public ConsoleTextLineView(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = new ConsoleContinuousCursor(console);
            _line = new ArrayPoolTextLine(console.BufferWidth);
        }

        ~ConsoleTextLineView() => Dispose();

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

            int width = _console.BufferWidth;
            int left = _console.CursorLeft + count;

            int tailLength = Length - Position;
            if (tailLength > 0)
            {
                var tail = Span.Slice(Position);
                _console.Write(tail);
                left += tailLength;
            }

            if (left % width == 0) count++;

            _console.Write(' ', count);
            _cursor.Move(-tailLength - count);
        }

        public void Type(char value)
        {
            int width = _console.BufferWidth;
            int left = _console.CursorLeft + 1;

            _console.Write(value);

            if (Position != Length)
            {
                var tail = Span.Slice(Position);
                _console.Write(tail);
                left += tail.Length;
                if (left % width == 0)
                {
                    FixCursor(' ');
                }
                _cursor.Move(-tail.Length);
            }
            else if (left % width == 0)
            {
                FixCursor(' ');
            }

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

            int width = _console.BufferWidth;
            int left = _console.CursorLeft + value.Length;

            _console.Write(value);

            if (Position != Length)
            {
                var tail = Span.Slice(Position);
                _console.Write(tail);
                left += tail.Length;
                if (left % width == 0)
                {
                    FixCursor(' ');
                }
                _cursor.Move(-tail.Length);
            }
            else if (left % width == 0)
            {
                FixCursor(' ');
            }

            _line.Type(value);
        }

        public void TypeOver(char value)
        {
            int width = _console.BufferWidth;
            int left = _console.CursorLeft + 1;

            _console.Write(value);
            if (left == width)
            {
                FixCursor(Position + 1 < Length ? Span[Position + 1] : ' ');
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
            int left = (_console.CursorLeft + value.Length) % width;

            _console.Write(value);
            _line.TypeOver(value);

            if (left == 0)
            {
                FixCursor(Position < Length ? Span[Position] : ' ');
            }
        }

        private void FixCursor(char nextChar)
        {
            _console.Write(nextChar);
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

            _console.Write(Span);

            if (left % width == 0) FixCursor(' ');
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
