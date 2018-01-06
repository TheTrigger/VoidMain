using System;
using System.Text;

namespace VoidMain.CommandLineIinterface.Internal
{
    public class CommandLineBuilder
    {
        private readonly StringBuilder _builder;
        private int _position;

        public int Position => _position;
        public int Length => _builder.Length;

        public char this[int index]
        {
            get => _builder[index];
            set => _builder[index] = value;
        }

        public CommandLineBuilder(int capacity = 256)
        {
            _builder = new StringBuilder(capacity);
        }

        public void Move(int offset)
        {
            int newPos = _position + offset;
            if (newPos < 0 || newPos > _builder.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            _position = newPos;
        }

        public void MoveTo(int newPos)
        {
            if (newPos < 0 || newPos > _builder.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(newPos));
            }
            _position = newPos;
        }

        public void Insert(char value)
        {
            if (_position == _builder.Length)
            {
                _builder.Append(value);
            }
            else
            {
                _builder.Insert(_position, value);
            }
            _position++;
        }

        public void Insert(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            if (_position == _builder.Length)
            {
                _builder.Append(value);
            }
            else
            {
                _builder.Insert(_position, value);
            }

            _position += value.Length;
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            int newPos = _position + count;
            if (newPos < 0 || newPos > _builder.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count < 0)
            {
                _position = newPos;
                count = -count;
            }

            _builder.Remove(_position, count);
        }

        public void Clear()
        {
            _builder.Clear();
            _position = 0;
        }

        public string ToString(int start, int length)
        {
            return _builder.ToString(start, length);
        }

        public string ToString(int start)
        {
            return _builder.ToString(start, _builder.Length - start);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
