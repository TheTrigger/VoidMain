using System;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleMaskedLineView : ILineView
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _consoleCursor;
        private readonly InMemoryLineView _line;
        private readonly char _maskSymbol;

        public LineViewType ViewType { get; }
        public int Position => _line.Position;
        public int Length => _line.Length;
        public char this[int index] => _line[index];

        public ConsoleMaskedLineView(IConsole console, IConsoleCursor consoleCursor, char maskSymbol)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _consoleCursor = consoleCursor ?? throw new ArgumentNullException(nameof(consoleCursor));
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
            _consoleCursor.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            int oldPos = Position;
            _line.MoveTo(newPos);
            _consoleCursor.Move(newPos - oldPos);
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            _line.Delete(count);

            int diff = Length - Position;
            if (count < 0)
            {
                count = -count;
                _consoleCursor.Move(diff - count);
            }
            else
            {
                _consoleCursor.Move(diff);
            }
            _console.Write(' ', count);
            _consoleCursor.Move(-diff - count);
        }

        public void Clear()
        {
            _consoleCursor.Move(-Position);
            _console.Write(' ', Length);
            _consoleCursor.Move(-Length);

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
                _consoleCursor.Move(diff);
                _console.Write(_maskSymbol);
                _consoleCursor.Move(-diff);
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
                _consoleCursor.Move(1);
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
                _consoleCursor.Move(diff);
                _console.Write(_maskSymbol, value.Length);
                _consoleCursor.Move(-diff);
            }

            _line.Type(value);
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            int diff = Length - Position;
            if (diff < value.Length)
            {
                _consoleCursor.Move(diff);
                _console.Write(_maskSymbol, value.Length - diff);
            }
            else
            {
                _consoleCursor.Move(value.Length);
            }

            _line.TypeOver(value);
        }
    }
}
