using System;
using System.Text;
using VoidMain.CommandLineInterface.IO.Console;

namespace VoidMain.CommandLineInterface.IO
{
    public class ConsoleAnsiColoredTextWriter : IColoredTextWriter
    {
        private const string CommandBegin = "\x1b";
        private const string CommandEnd = "m";
        private const string ColorDelimiter = ";";
        private const string ForegroundCode = "[38;2;";
        private const string BackgroundCode = "[48;2;";
        private const string ResetColorsCommand = CommandBegin + "[0" + CommandEnd;

        private readonly IConsole _console;
        private readonly StringBuilder _command;

        public ConsoleAnsiColoredTextWriter(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _command = new StringBuilder(64);
        }

        public void Write(Color foreground, Color background, char value)
        {
            ChangeColor(foreground, ForegroundCode);
            ChangeColor(background, BackgroundCode);

            _console.Write(value);

            ResetColors();
        }

        public void Write(Color foreground, Color background, char value, int count)
        {
            ChangeColor(foreground, ForegroundCode);
            ChangeColor(background, BackgroundCode);

            _console.Write(value, count);

            ResetColors();
        }

        public void Write(Color foreground, Color background, string value)
        {
            ChangeColor(foreground, ForegroundCode);
            ChangeColor(background, BackgroundCode);

            _console.Write(value);

            ResetColors();
        }

        public void Write(Color foreground, Color background, object value)
        {
            ChangeColor(foreground, ForegroundCode);
            ChangeColor(background, BackgroundCode);

            _console.Write(value);

            ResetColors();
        }

        public void WriteLine()
        {
            _console.WriteLine();
        }

        private void ChangeColor(Color color, string colorCode)
        {
            if (color == null) return;

            _command.Append(CommandBegin)
                .Append(colorCode)
                .Append(color.R)
                .Append(ColorDelimiter)
                .Append(color.G)
                .Append(ColorDelimiter)
                .Append(color.B)
                .Append(CommandEnd);

            _console.Write(_command.ToString());
            _command.Clear();
        }

        private void ResetColors()
        {
            _console.Write(ResetColorsCommand);
        }
    }
}
