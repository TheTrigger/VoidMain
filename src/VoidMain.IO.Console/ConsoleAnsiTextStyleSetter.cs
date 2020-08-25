using VoidMain.Text.Style;

namespace VoidMain.IO.Console
{
    public class ConsoleAnsiTextStyleSetter : IConsoleStyleSetter<TextStyle>
    {
        private readonly ConsoleAnsiWriter _ansi;

        public ConsoleAnsiTextStyleSetter(IConsole console)
        {
            _ansi = new ConsoleAnsiWriter(console);
        }

        public void ClearStyle() => _ansi.WriteReset();

        public void SetStyle(TextStyle style)
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
