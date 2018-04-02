using System;
using System.Text;

namespace VoidMain.CommandLineIinterface.Internal
{
    public class CommandLineBuilder
    {
        private readonly StringBuilder _builder;
        private string _cache;

        public int Position { get; private set; }
        public int Length => _builder.Length;

        public char this[int index]
        {
            get => _builder[index];
            set
            {
                _builder[index] = value;
                _cache = null;
            }
        }

        public CommandLineBuilder(int capacity = 256)
        {
            _builder = new StringBuilder(capacity);
        }

        public void Move(int offset)
        {
            int newPos = Position + offset;
            if (newPos < 0 || newPos > _builder.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            Position = newPos;
        }

        public void MoveTo(int newPos)
        {
            if (newPos < 0 || newPos > _builder.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(newPos));
            }
            Position = newPos;
        }

        public void Insert(char value)
        {
            if (Position == _builder.Length)
            {
                _builder.Append(value);
            }
            else
            {
                _builder.Insert(Position, value);
            }
            Position++;
            _cache = null;
        }

        public void Insert(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            if (Position == _builder.Length)
            {
                _builder.Append(value);
            }
            else
            {
                _builder.Insert(Position, value);
            }

            Position += value.Length;
            _cache = null;
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            int newPos = Position + count;
            if (newPos < 0 || newPos > _builder.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count < 0)
            {
                Position = newPos;
                count = -count;
            }

            _builder.Remove(Position, count);
            _cache = null;
        }

        public void Clear()
        {
            _builder.Clear();
            Position = 0;
            _cache = null;
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
            return _cache ?? (_cache = _builder.ToString());
        }
    }
}
