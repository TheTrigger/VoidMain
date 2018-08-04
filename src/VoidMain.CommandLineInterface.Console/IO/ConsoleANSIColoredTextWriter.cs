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
            ChangeForeground(foreground);
            ChangeBackground(background);

            _console.Write(value);

            ResetColors();
        }

        public void Write(Color foreground, Color background, char value, int count)
        {
            if (count <= 0) return;

            ChangeForeground(foreground);
            ChangeBackground(background);

            _console.Write(value, count);

            ResetColors();
        }

        public void Write(Color foreground, Color background, string value)
        {
            ChangeForeground(foreground);
            ChangeBackground(background);

            _console.Write(value);

            ResetColors();
        }

        public void Write(Color foreground, Color background, object value)
        {
            ChangeForeground(foreground);
            ChangeBackground(background);

            _console.Write(value);

            ResetColors();
        }

        public void WriteLine()
        {
            _console.WriteLine();
        }

        private void ChangeForeground(Color foreground)
        {
            if (foreground == null) return;
            string colorCommand = GetColorCommand(foreground, ForegroundCode);
            _console.Write(colorCommand);
        }

        private void ChangeBackground(Color background)
        {
            if (background == null) return;
            string colorCommand = GetColorCommand(background, BackgroundCode);
            _console.Write(colorCommand);
        }

        private string GetColorCommand(Color color, string colorCode)
        {
            _command.Append(CommandBegin)
                .Append(colorCode)
                .Append(color.R)
                .Append(ColorDelimiter)
                .Append(color.G)
                .Append(ColorDelimiter)
                .Append(color.B)
                .Append(CommandEnd);

            string colorCommand = _command.ToString();
            _command.Clear();

            return colorCommand;
        }

        private void ResetColors()
        {
            _console.Write(ResetColorsCommand);
        }
    }
}
