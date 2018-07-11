using System;

namespace VoidMain.CommandLineIinterface.IO.Console
{
    public class TerminalCursor : IConsoleCursor
    {
        private readonly IConsole _console;

        public TerminalCursor(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public void Move(int offset)
        {
            if (offset == 0) return;

            // Terminal doesn't have BufferWidth,
            // but CursorLeft can't be more than buffer width
            // and CursorLeft doesn't throw.

            if (offset > 0)
            {
                int left = _console.CursorLeft + offset;
                _console.CursorLeft = left;
                if (_console.CursorLeft < left)
                {
                    int bufferWidth = _console.CursorLeft + 1;
                    _console.CursorTop += left / bufferWidth;
                    _console.CursorLeft = left % bufferWidth;
                }
            }
            else
            {
                int left = _console.CursorLeft + offset;
                if (left < 0)
                {
                    _console.CursorLeft = Int32.MaxValue;
                    int bufferWidth = _console.CursorLeft + 1;
                    _console.CursorTop -= -left / bufferWidth + 1;
                    _console.CursorLeft = bufferWidth + left % bufferWidth;
                }
                else
                {
                    _console.CursorLeft = left;
                }
            }
        }
    }
}
