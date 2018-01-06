using System;

namespace VoidMain.CommandLineIinterface.IO.Console.Internal
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

            int left = _console.CursorLeft + offset;
            if (0 <= left && left < _console.BufferWidth)
            {
                _console.CursorLeft = left;
                return;
            }

            if (offset > 0)
            {
                _console.CursorTop += left / _console.BufferWidth;
                _console.CursorLeft = left % _console.BufferWidth;
            }
            else
            {
                _console.CursorTop += left / _console.BufferWidth - 1;
                _console.CursorLeft = _console.BufferWidth + left % _console.BufferWidth;
            }
        }
    }
}
