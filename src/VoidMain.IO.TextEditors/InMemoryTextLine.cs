using System;
using System.Buffers;

namespace VoidMain.IO.TextEditors
{
    public class InMemoryTextLine : ITextLine, IDisposable
    {
        private char[] _text;
        private int _position;

        public int Length { get; private set; }

        public int Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > Length) Throw();
                _position = value;
                static void Throw() => throw new ArgumentOutOfRangeException(nameof(Position));
            }
        }

        public ReadOnlySpan<char> Span => _text.AsSpan(0, Length);

        public uint ContentVersion { get; private set; }

        public InMemoryTextLine(int minCapacity)
        {
            _text = ArrayPool<char>.Shared.Rent(minCapacity);
            _position = 0;
            Length = 0;
            ContentVersion = 0;
        }

        ~InMemoryTextLine() => Dispose();

        public void Dispose()
        {
            if (_text == null) return;
            ArrayPool<char>.Shared.Return(_text, clearArray: true);
            _text = null!;
        }

        public override string ToString()
            => new string(_text, 0, Length);

        public void Clear()
        {
            _position = 0;
            Length = 0;
            unchecked { ContentVersion++; }
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            int newPos = _position + count;
            if (newPos < 0 || newPos > Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count < 0)
            {
                _position = newPos;
                count = -count;
            }

            Length -= count;
            if (_position != Length)
            {
                Array.Copy(_text, _position + count, _text, _position, Length - _position);
            }
            unchecked { ContentVersion++; }
        }

        public void Type(char value)
        {
            if (Length == _text.Length)
            {
                Resize(Length + 1);
            }

            if (_position != Length)
            {
                Array.Copy(_text, _position, _text, _position + 1, Length - _position);
            }

            _text[_position] = value;
            _position++;
            Length++;
            unchecked { ContentVersion++; }
        }

        public void Type(string value)
        {
            if (value == null) return;
            Type(value.AsSpan());
        }

        public void Type(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty) return;

            int newLen = Length + value.Length;
            if (newLen > _text.Length)
            {
                Resize(newLen);
            }

            if (_position != Length)
            {
                Array.Copy(_text, _position, _text, _position + value.Length, Length - _position);
            }

            value.CopyTo(_text.AsSpan(_position, value.Length));
            _position += value.Length;
            Length = newLen;
            unchecked { ContentVersion++; }
        }

        public void TypeOver(char value)
        {
            if (_position == _text.Length)
            {
                Resize(Length + 1);
            }

            _text[_position] = value;
            _position++;
            if (_position > Length)
            {
                Length = _position;
            }
            unchecked { ContentVersion++; }
        }

        public void TypeOver(string value)
        {
            if (value == null) return;
            TypeOver(value.AsSpan());
        }

        public void TypeOver(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty) return;

            int newPos = _position + value.Length;
            if (newPos > _text.Length)
            {
                Resize(newPos);
            }

            value.CopyTo(_text.AsSpan(_position, value.Length));
            _position = newPos;
            if (_position > Length)
            {
                Length = _position;
            }
            unchecked { ContentVersion++; }
        }

        private void Resize(int minLength)
        {
            if (minLength <= Length) return;
            char[] newText = ArrayPool<char>.Shared.Rent(minLength);
            Array.Copy(_text, 0, newText, 0, Length);
            ArrayPool<char>.Shared.Return(_text, clearArray: true);
            _text = newText;
        }

        public void ClearState()
        {
            Length = 0;
            _position = 0;
            ContentVersion = 0;
        }

        public void SetState(ReadOnlySpan<char> text, int position)
        {
            if (position < 0 || position > text.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            if (text.Length > _text.Length)
            {
                Resize(text.Length);
            }

            text.CopyTo(_text.AsSpan());
            Length = text.Length;
            _position = position;
            ContentVersion = 0;
        }
    }
}
