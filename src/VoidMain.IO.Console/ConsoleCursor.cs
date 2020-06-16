using System;

namespace VoidMain.IO.Console
{
    public class ConsoleCursor
    {
        private readonly IConsole _console;

        public ConsoleCursor(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void Move(int offset)
        {
            if (offset == 0) return;

            int width = _console.BufferWidth;
            int top = _console.CursorTop;
            int left = _console.CursorLeft;

            int leftOffset = left + offset;

            if (leftOffset >= width)
            {
                top = Math.DivRem(leftOffset, width, out left);
            }
            else if (leftOffset < 0)
            {
                int right = width - 1;
                int rigthOffset = right - left - offset;
                top -= Math.DivRem(rigthOffset, width, out leftOffset);
                left = right - leftOffset;
            }
            else
            {
                left = leftOffset;
            }

            _console.SetCursorPosition(top, left);
        }
    }
}
