using VoidMain.Text;

namespace VoidMain.IO.Console
{
    public class ConsoleStyledTextWriter : ConsoleTextWriter, IStyledTextWriter<TextStyle>
    {
        private readonly ConsoleAnsiWriter _ansi;

        public ConsoleStyledTextWriter(IConsole console)
                : base(console)
        {
            _ansi = new ConsoleAnsiWriter(console);
        }

        public void ClearStyle() => _ansi.WriteReset();

        public void WriteStyle(TextStyle style)
        {
            if (style.Foreground is Color foreground)
            {
                _ansi.WriteForeground(foreground);
            }
            else
            {
                _ansi.WriteDefaultForeground();
            }

            if (style.Background is Color background)
            {
                _ansi.WriteBackground(background);
            }
            else
            {
                _ansi.WriteDefaultBackground();
            }
        }
    }
}
