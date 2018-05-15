using System;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleMaskedLineView : ILineView
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly InMemoryLineView _line;
        private readonly char _maskSymbol;

        public LineViewType ViewType { get; }
        public int Position => _line.Position;
        public int Length => _line.Length;
        public char this[int index] => _line[index];

        public ConsoleMaskedLineView(IConsole console, IConsoleCursor cursor, char maskSymbol)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _line = new InMemoryLineView();
            ViewType = LineViewType.Masked;
            _maskSymbol = maskSymbol;
        }

        public string ToString(int start, int length) => _line.ToString(start, length);
        public string ToString(int start) => _line.ToString(start);
        public override string ToString() => _line.ToString();

        public void Move(int offset)
        {
            _line.Move(offset);
            _cursor.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            int oldPos = Position;
            _line.MoveTo(newPos);
            _cursor.Move(newPos - oldPos);
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            _line.Delete(count);

            int diff = Length - Position;
            if (count < 0)
            {
                count = -count;
                _cursor.Move(diff - count);
            }
            else
            {
                _cursor.Move(diff);
            }
            _console.Write(' ', count);
            _cursor.Move(-diff - count);
        }

        public void Clear()
        {
            _cursor.Move(-Position);
            _console.Write(' ', Length);
            _cursor.Move(-Length);

            _line.Clear();
        }

        public void Type(char value)
        {
            if (Position == Length)
            {
                _console.Write(_maskSymbol);
            }
            else
            {
                int diff = Length - Position;
                _cursor.Move(diff);
                _console.Write(_maskSymbol);
                _cursor.Move(-diff);
            }

            _line.Type(value);
        }

        public void TypeOver(char value)
        {
            if (Position == Length)
            {
                _console.Write(_maskSymbol);
            }
            else
            {
                _cursor.Move(1);
            }

            _line.TypeOver(value);
        }

        public void Type(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            if (Position == Length)
            {
                _console.Write(_maskSymbol, value.Length);
            }
            else
            {
                int diff = Length - Position;
                _cursor.Move(diff);
                _console.Write(_maskSymbol, value.Length);
                _cursor.Move(-diff);
            }

            _line.Type(value);
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            int diff = Length - Position;
            if (diff < value.Length)
            {
                _cursor.Move(diff);
                _console.Write(_maskSymbol, value.Length - diff);
            }
            else
            {
                _cursor.Move(value.Length);
            }

            _line.TypeOver(value);
        }
    }
}
