using System;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface.IO
{
    public class ConsoleColoredTextWriter : IColoredTextWriter
    {
        private readonly IConsole _console;
        private readonly IConsoleColorConverter _colorConverter;

        public ConsoleColoredTextWriter(IConsole console, IConsoleColorConverter colorConverter)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _colorConverter = colorConverter ?? throw new ArgumentNullException(nameof(colorConverter));
        }

        public void Write(Color foreground, Color background, string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return;
            }

            bool fgChanged = ChangeForeground(foreground, out var prevFg);
            bool bgChanged = ChangeBackground(background, out var prevBg);

            _console.Write(value);

            if (fgChanged) _console.ForegroundColor = prevFg;
            if (bgChanged) _console.BackgroundColor = prevBg;
        }

        public void Write(Color foreground, Color background, object value)
        {
            if (value == null)
            {
                return;
            }

            bool fgChanged = ChangeForeground(foreground, out var prevFg);
            bool bgChanged = ChangeBackground(background, out var prevBg);

            _console.Write(value);

            if (fgChanged) _console.ForegroundColor = prevFg;
            if (bgChanged) _console.BackgroundColor = prevBg;
        }

        public void Write(Color foreground, Color background, char value)
        {
            bool fgChanged = ChangeForeground(foreground, out var prevFg);
            bool bgChanged = ChangeBackground(background, out var prevBg);

            _console.Write(value);

            if (fgChanged) _console.ForegroundColor = prevFg;
            if (bgChanged) _console.BackgroundColor = prevBg;
        }

        public void Write(Color foreground, Color background, char value, int length)
        {
            if (length <= 0)
            {
                return;
            }

            bool fgChanged = ChangeForeground(foreground, out var prevFg);
            bool bgChanged = ChangeBackground(background, out var prevBg);

            for (int i = 0; i < length; i++)
            {
                _console.Write(value);
            }

            if (fgChanged) _console.ForegroundColor = prevFg;
            if (bgChanged) _console.BackgroundColor = prevBg;
        }

        public void WriteLine()
        {
            _console.WriteLine();
        }

        private bool ChangeForeground(Color foreground, out ConsoleColor prevFg)
        {
            prevFg = _console.ForegroundColor;
            if (foreground == null)
            {
                return false;
            }

            _console.ForegroundColor = ConvertColor(foreground);
            return true;
        }

        private bool ChangeBackground(Color background, out ConsoleColor prevBg)
        {
            prevBg = _console.BackgroundColor;
            if (background == null)
            {
                return false;
            }

            _console.BackgroundColor = ConvertColor(background);
            return true;
        }

        private ConsoleColor ConvertColor(Color color)
        {
            if (_colorConverter.TryConvert(color, out var consoleColor))
            {
                return consoleColor;
            }
            throw new NotSupportedException($"Color {color} is not supported.");
        }
    }
}
