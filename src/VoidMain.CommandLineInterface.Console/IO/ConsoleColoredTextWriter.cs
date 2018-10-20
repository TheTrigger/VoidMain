using System;
using VoidMain.CommandLineInterface.IO.Console;

namespace VoidMain.CommandLineInterface.IO
{
    public class ConsoleColoredTextWriter : ConsoleTextWriter, IColoredTextWriter
    {
        private readonly IConsoleColorConverter _colorConverter;

        public ConsoleColoredTextWriter(IConsole console, IConsoleColorConverter colorConverter)
            : base(console)
        {
            _colorConverter = colorConverter ?? throw new ArgumentNullException(nameof(colorConverter));
        }

        public void SetColors(Color foreground, Color background)
        {
            ChangeForeground(foreground);
            ChangeBackground(background);
        }

        public void ResetColors()
        {
            _console.ResetColors();
        }

        private void ChangeForeground(Color foreground)
        {
            if (foreground == null) return;
            _console.ForegroundColor = ConvertColor(foreground);
        }

        private void ChangeBackground(Color background)
        {
            if (background == null) return;
            _console.BackgroundColor = ConvertColor(background);
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
