using System;
using VoidMain.Text.Style;

namespace VoidMain.IO.Console
{
    public class ConsoleAnsiWriter
    {
        private const string Reset = "\x1B[0m";
        private const string DefaultForeground = "\x1B[39m";
        private const string DefaultBackground = "\x1B[49m";

        private readonly IConsole _console;
        private readonly ReadOnlyMemory<char> _format;
        private readonly Memory<char> _setColor;
        private readonly Memory<char> _redSpan;
        private readonly Memory<char> _greenSpan;
        private readonly Memory<char> _blueSpan;

        public ConsoleAnsiWriter(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _format = "D3".AsMemory();
            _setColor = "\x1B[38;2;000;000;000m".ToCharArray();
            _redSpan = _setColor.Slice(7, 3);
            _greenSpan = _setColor.Slice(11, 3);
            _blueSpan = _setColor.Slice(15, 3);
        }

        public void WriteForeground(Color color) => WriteColor(color, '3');

        public void WriteBackground(Color color) => WriteColor(color, '4');

        public void WriteDefaultForeground() => _console.Write(DefaultForeground);

        public void WriteDefaultBackground() => _console.Write(DefaultBackground);

        public void WriteReset() => _console.Write(Reset);

        private void WriteColor(Color color, char code)
        {
            var span = _setColor.Span;
            span[2] = code;

            var format = _format.Span;
            color.R.TryFormat(_redSpan.Span, out _, format);
            color.G.TryFormat(_greenSpan.Span, out _, format);
            color.B.TryFormat(_blueSpan.Span, out _, format);

            _console.Write(span);
        }
    }
}
