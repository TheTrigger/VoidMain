using System;

namespace VoidMain.CommandLineIinterface.Console
{
    public class ConsoleCommandLineHiddenView : ICommandLineView
    {
        private readonly CommandLineBuilder _lineBuilder;

        public ConsoleCommandLineHiddenView()
        {
            _lineBuilder = new CommandLineBuilder();
            ViewType = CommandLineViewType.Hidden;
            MaskSymbol = Char.MinValue;
        }

        public CommandLineViewType ViewType { get; private set; }
        public char MaskSymbol { get; private set; }

        public int Position => _lineBuilder.Position;
        public int Length => _lineBuilder.Length;
        public char this[int index] => _lineBuilder[index];

        public string ToString(int start, int length) => _lineBuilder.ToString(start, length);
        public string ToString(int start) => _lineBuilder.ToString(start);
        public override string ToString() => _lineBuilder.ToString();

        public void Move(int offset)
        {
            _lineBuilder.Move(offset); // Throws if out of range
        }

        public void MoveTo(int newPos)
        {
            _lineBuilder.MoveTo(newPos); // Throws if out of range
        }

        public void Delete(int count)
        {
            if (count == 0) return;
            _lineBuilder.Delete(count); // Throws if out of range
        }

        public void ClearAll()
        {
            _lineBuilder.Clear();
        }

        public void Type(char value)
        {
            _lineBuilder.Insert(value);
        }

        public void TypeOver(char value)
        {
            if (_lineBuilder.Position < _lineBuilder.Length)
            {
                _lineBuilder[_lineBuilder.Position] = value;
                _lineBuilder.Move(1);
            }
            else
            {
                _lineBuilder.Insert(value);
            }
        }

        public void Type(string value)
        {
            if (String.IsNullOrEmpty(value)) return;
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
            if (offset < value.Length)
            {
                _lineBuilder.Insert(value.Substring(offset));
            }
        }
    }
}
