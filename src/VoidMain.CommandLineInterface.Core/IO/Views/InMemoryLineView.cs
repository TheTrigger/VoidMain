using System;
using System.Text;

namespace VoidMain.CommandLineInterface.IO.Views
{
    public class InMemoryLineView : ILineView, IReusableLineView
    {
        private readonly StringBuilder _lineBuilder;
        private string _cache;

        public LineViewType ViewType { get; }
        public int Position { get; private set; }
        public int Length => _lineBuilder.Length;
        public char this[int index] => _lineBuilder[index];

        public InMemoryLineView()
        {
            _lineBuilder = new StringBuilder();
            ViewType = LineViewType.Hidden;
        }

        public string ToString(int start, int length)
        {
            return start == 0 && length == _lineBuilder.Length
                ? ToString()
                : _lineBuilder.ToString(start, length);
        }

        public string ToString(int start)
        {
            return start == 0
                ? ToString()
                : _lineBuilder.ToString(start, Length - start);
        }

        public override string ToString()
        {
            return _cache ?? (_cache = _lineBuilder.ToString());
        }

        public void Move(int offset)
        {
            int newPos = Position + offset;
            if (newPos < 0 || newPos > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            Position = newPos;
        }

        public void MoveTo(int newPos)
        {
            if (newPos < 0 || newPos > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(newPos));
            }
            Position = newPos;
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            int newPos = Position + count;
            if (newPos < 0 || newPos > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count < 0)
            {
                Position = newPos;
                count = -count;
            }

            _lineBuilder.Remove(Position, count);
            _cache = null;
        }

        public void Clear()
        {
            _lineBuilder.Clear();
            Position = 0;
            _cache = null;
        }

        public void Type(char value)
        {
            if (Position == Length)
            {
                _lineBuilder.Append(value);
            }
            else
            {
                _lineBuilder.Insert(Position, value);
            }

            Position++;
            _cache = null;
        }

        public void TypeOver(char value)
        {
            if (Position == Length)
            {
                _lineBuilder.Append(value);
            }
            else
            {
                _lineBuilder[Position] = value;
            }

            Position++;
            _cache = null;
        }

        public void Type(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            if (Position == Length)
            {
                _lineBuilder.Append(value);
            }
            else
            {
                _lineBuilder.Insert(Position, value);
            }

            Position += value.Length;
            _cache = null;
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            int offset = 0;
            while (Position < Length && offset < value.Length)
            {
                _lineBuilder[Position] = value[offset];
                Position++;
                offset++;
            }
            if (offset < value.Length)
            {
                int length = value.Length - offset;
                _lineBuilder.Append(value, offset, length);
                Position += length;
            }
            _cache = null;
        }

        public void SetState(string line, int position)
        {
            if (line == null)
            {
                throw new ArgumentNullException(nameof(line));
            }
            if (position < 0 || position > line.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            _cache = null;
            _lineBuilder.Clear();
            _lineBuilder.Append(line);
            Position = position;
        }

        public void ClearState()
        {
            _cache = null;
            _lineBuilder.Clear();
            Position = 0;
        }

        public void RenderState()
        {
            // do nothing
        }
    }
}
