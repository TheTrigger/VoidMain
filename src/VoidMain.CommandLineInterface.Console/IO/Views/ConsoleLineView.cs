using System;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleLineView : ILineView, IReusableLineView
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _consoleCursor;
        private readonly InMemoryLineView _line;

        public LineViewType ViewType { get; }
        public int Position => _line.Position;
        public int Length => _line.Length;
        public char this[int index] => _line[index];

        public ConsoleLineView(IConsole console, IConsoleCursor consoleCursor)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _consoleCursor = consoleCursor ?? throw new ArgumentNullException(nameof(consoleCursor));
            _line = new InMemoryLineView();
            ViewType = LineViewType.Normal;
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

            if (count < 0)
            {
                _consoleCursor.Move(count);
                count = -count;
            }

            if (Position != Length)
            {
                string tail = ToString(Position);
                _console.Write(tail);
            }
            _console.Write(' ', count);
            _consoleCursor.Move(Position - Length - count);
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
            _console.Write(value);
            if (Position != Length)
            {
                string tail = ToString(Position);
                _console.Write(tail);
                _consoleCursor.Move(-tail.Length);
            }

            _line.Type(value);
        }

        public void TypeOver(char value)
        {
            _console.Write(value);
            _line.TypeOver(value);
        }

        public void Type(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            _console.Write(value);
            if (Position != Length)
            {
                string tail = ToString(Position);
                _console.Write(tail);
                _consoleCursor.Move(-tail.Length);
            }

            _line.Type(value);
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            _console.Write(value);
            _line.TypeOver(value);
        }

        public void SetState(string line, int position)
        {
            _line.SetState(line, position);
        }

        public void ClearState()
        {
            _line.ClearState();
        }

        public void RenderState()
        {
            _consoleCursor.Move(-Position);
            _console.Write(ToString());
            _consoleCursor.Move(Position - Length);
        }
    }
}
