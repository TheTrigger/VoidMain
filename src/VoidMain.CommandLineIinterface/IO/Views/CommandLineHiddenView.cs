using System;
using VoidMain.CommandLineIinterface.Internal;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class CommandLineHiddenView : ICommandLineView
    {
        private readonly CommandLineBuilder _lineBuilder;

        public CommandLineHiddenView()
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
            // Throws if out of range
            _lineBuilder.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            // Throws if out of range
            _lineBuilder.MoveTo(newPos);
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            // Throws if out of range
            _lineBuilder.Delete(count);
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
