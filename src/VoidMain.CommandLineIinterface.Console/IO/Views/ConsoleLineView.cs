﻿using System;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleLineView : ILineView
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly InMemoryLineView _line;

        public LineViewType ViewType { get; }
        public int Position => _line.Position;
        public int Length => _line.Length;
        public char this[int index] => _line[index];

        public ConsoleLineView(IConsole console, IConsoleCursor cursor)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _line = new InMemoryLineView();
            ViewType = LineViewType.Normal;
        }

        public string ToString(int start, int length) => _line.ToString(start, length);
        public string ToString(int start) => _line.ToString(start);
        public override string ToString() => _line.ToString();

        public void Move(int offset)
        {
            _line.Move(offset);
            _cursor.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            int oldPos = Position;
            _line.MoveTo(newPos);
            _cursor.Move(newPos - oldPos);
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            _line.Delete(count);

            if (count < 0)
            {
                _cursor.Move(count);
                count = -count;
            }

            if (Position != Length)
            {
                string tail = ToString(Position);
                _console.Write(tail);
            }
            _console.Write(' ', count);
            _cursor.Move(Position - Length - count);
        }

        public void Clear()
        {
            _cursor.Move(-Position);
            _console.Write(' ', Length);
            _cursor.Move(-Length);

            _line.Clear();
        }

        public void Type(char value)
        {
            _console.Write(value);
            if (Position != Length)
            {
                string tail = ToString(Position);
                _console.Write(tail);
                _cursor.Move(-tail.Length);
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
                _cursor.Move(-tail.Length);
            }

            _line.Type(value);
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            _console.Write(value);
            _line.TypeOver(value);
        }
    }
}