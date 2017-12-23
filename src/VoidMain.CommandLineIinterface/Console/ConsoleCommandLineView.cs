using System;

namespace VoidMain.CommandLineIinterface.Console
{
    class ConsoleCommandLineView : ICommandLineViewManager, ICommandLineView
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly CommandLineBuilder _lineBuilder;

        public ConsoleCommandLineView(IConsole console, IConsoleCursor cursor)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _lineBuilder = new CommandLineBuilder();
        }

        public ICommandLineView GetView()
        {
            return this;
        }

        public void StartChanges()
        {
            // You can start to collect all changes to the view.
        }

        public void EndChanges()
        {
            // You can commit all changes to the view.
        }

        public void ResetState()
        {
            _lineBuilder.Clear();
        }

        public int Position => _lineBuilder.Position;
        public int Length => _lineBuilder.Length;
        public char this[int index] => _lineBuilder[index];

        public string ToString(int start, int length) => _lineBuilder.ToString(start, length);
        public string ToString(int start) => _lineBuilder.ToString(start);
        public override string ToString() => _lineBuilder.ToString();

        public void Move(int offset)
        {
            _lineBuilder.Move(offset); // Throw if out of range
            _cursor.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            int oldPos = _lineBuilder.Position;
            _lineBuilder.MoveTo(newPos); // Throw if out of range
            _cursor.Move(newPos - oldPos);
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            _lineBuilder.Delete(count);

            if (count < 0)
            {
                _cursor.Move(count);
                count = -count;
            }

            if (_lineBuilder.Position != _lineBuilder.Length)
            {
                string tail = _lineBuilder.ToString(_lineBuilder.Position);
                _console.Write(tail);
            }
            FillBlank(count);
            _cursor.Move(_lineBuilder.Position - _lineBuilder.Length - count);
        }

        public void ClearAll()
        {
            _cursor.Move(-_lineBuilder.Position);
            FillBlank(_lineBuilder.Length);
            _cursor.Move(-_lineBuilder.Length);

            _lineBuilder.Clear();
        }

        private void FillBlank(int length)
        {
            for (int i = 0; i < length; i++)
            {
                _console.Write(' ');
            }
        }

        public void Type(char value)
        {
            _console.Write(value);
            if (_lineBuilder.Position != _lineBuilder.Length)
            {
                string tail = _lineBuilder.ToString(_lineBuilder.Position);
                _console.Write(tail);
                _cursor.Move(-tail.Length);
            }
            _lineBuilder.Insert(value);
        }

        public void TypeOver(char value)
        {
            _console.Write(value);
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

            _console.Write(value);
            if (_lineBuilder.Position != _lineBuilder.Length)
            {
                string tail = _lineBuilder.ToString(_lineBuilder.Position);
                _console.Write(tail);
                _cursor.Move(-tail.Length);
            }
            _lineBuilder.Insert(value);
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            _console.Write(value);

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
