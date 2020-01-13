using System.Text;
using VoidMain.CommandLineInterface.IO.Console;

namespace VoidMain.CommandLineInterface.IO
{
    public class ConsoleAnsiColoredTextWriter : ConsoleTextWriter, IColoredTextWriter
    {
        private const string CommandBegin = "\x1b";
        private const string CommandEnd = "m";
        private const string ColorDelimiter = ";";
        private const string ForegroundCode = "[38;2;";
        private const string BackgroundCode = "[48;2;";
        private const string ResetColorsCommand = CommandBegin + "[0" + CommandEnd;

        private readonly StringBuilder _command;

        public ConsoleAnsiColoredTextWriter(IConsole console)
            : base(console)
        {
            _command = new StringBuilder(64);
        }

        public void SetColors(Color? foreground, Color? background)
        {
            ChangeForeground(foreground);
            ChangeBackground(background);
        }

        public void ResetColors()
        {
            _console.Write(ResetColorsCommand);
        }

        private void ChangeForeground(Color? foreground)
        {
            if (foreground == null) return;
            string colorCommand = GetColorCommand(foreground.Value, ForegroundCode);
            _console.Write(colorCommand);
        }

        private void ChangeBackground(Color? background)
        {
            if (background == null) return;
            string colorCommand = GetColorCommand(background.Value, BackgroundCode);
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
    }
}
