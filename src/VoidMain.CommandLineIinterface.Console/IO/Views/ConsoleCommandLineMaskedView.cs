using System;
using VoidMain.CommandLineIinterface.Internal;
using VoidMain.CommandLineIinterface.IO.Console.Internal;

namespace VoidMain.CommandLineIinterface.IO.Views.Console
{
    public class ConsoleCommandLineMaskedView : ICommandLineView
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly CommandLineBuilder _lineBuilder;

        public ConsoleCommandLineMaskedView(IConsole console, IConsoleCursor cursor, char maskSymbol)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _lineBuilder = new CommandLineBuilder();
            ViewType = CommandLineViewType.Masked;
            MaskSymbol = maskSymbol;
        }

        public CommandLineViewType ViewType { get; }
        public char MaskSymbol { get; }

        public int Position => _lineBuilder.Position;
        public int Length => _lineBuilder.Length;
        public char this[int index] => _lineBuilder[index];

        public string ToString(int start, int length) => _lineBuilder.ToString(start, length);
        public string ToString(int start) => _lineBuilder.ToString(start);
        public override string ToString() => _lineBuilder.ToString();

        public void Move(int offset)
        {
            // Throws if out of range
            _lineBuilder.Move(offset);
            _cursor.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            int oldPos = _lineBuilder.Position;
            // Throws if out of range
            _lineBuilder.MoveTo(newPos);
            _cursor.Move(newPos - oldPos);
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            // Throws if out of range
            _lineBuilder.Delete(count);

            int diff = _lineBuilder.Length - _lineBuilder.Position;
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

        public void ClearAll()
        {
            _cursor.Move(-_lineBuilder.Position);
            _console.Write(' ', _lineBuilder.Length);
            _cursor.Move(-_lineBuilder.Length);

            _lineBuilder.Clear();
        }

        public void Type(char value)
        {
            if (_lineBuilder.Position == _lineBuilder.Length)
            {
                _console.Write(MaskSymbol);
            }
            else
            {
                int diff = _lineBuilder.Length - _lineBuilder.Position;
                _cursor.Move(diff);
                _console.Write(MaskSymbol);
                _cursor.Move(-diff);
            }

            _lineBuilder.Insert(value);
        }

        public void TypeOver(char value)
        {
            if (_lineBuilder.Position < _lineBuilder.Length)
            {
                _lineBuilder[_lineBuilder.Position] = value;
                _lineBuilder.Move(1);
                _cursor.Move(1);
            }
            else
            {
                _lineBuilder.Insert(value);
                _console.Write(MaskSymbol);
            }
        }

        public void Type(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            if (_lineBuilder.Position == _lineBuilder.Length)
            {
                _console.Write(MaskSymbol, value.Length);
            }
            else
            {
                int diff = _lineBuilder.Length - _lineBuilder.Position;
                _cursor.Move(diff);
                _console.Write(MaskSymbol, value.Length);
                _cursor.Move(-diff);
            }

            _lineBuilder.Insert(value);
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            int offset = 0;
            while (_lineBuilder.Position < _lineBuilder.Length && offset < value.Length)
            {
                _lineBuilder[_lineBuilder.Position] = value[offset];
                _lineBuilder.Move(1);
                offset++;
            }
            _cursor.Move(offset);
            if (offset < value.Length)
            {
                value = value.Substring(offset);
                _lineBuilder.Insert(value);
                _console.Write(MaskSymbol, value.Length);
            }
        }
    }
}
