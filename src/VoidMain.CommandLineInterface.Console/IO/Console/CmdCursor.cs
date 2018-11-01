using System;

namespace VoidMain.CommandLineInterface.IO.Console
{
    public class CmdCursor : IConsoleCursor
    {
        private readonly IConsole _console;

        public CmdCursor(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void Move(int offset)
        {
            if (offset == 0) return;

            int lineWidth = _console.BufferWidth;
            int cursorTop = _console.CursorTop;
            int cursorLeft = _console.CursorLeft;

            int leftOffset = cursorLeft + offset;
            if (0 <= leftOffset && leftOffset < lineWidth)
            {
                _console.SetCursorPosition(cursorTop, leftOffset);
                return;
            }


            if (offset > 0)
            {
                cursorTop += leftOffset / lineWidth;
                cursorLeft = leftOffset % lineWidth;
            }
            else
            {
                int rightStart = lineWidth - 1;
                int rigthOffset = rightStart - cursorLeft - offset;
                cursorTop -= rigthOffset / lineWidth;
                cursorLeft = rightStart - rigthOffset % lineWidth;
            }

            _console.SetCursorPosition(cursorTop, cursorLeft);
        }
    }
}
