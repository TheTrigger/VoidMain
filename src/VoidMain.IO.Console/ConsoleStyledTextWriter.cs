using System;
using System.Runtime.CompilerServices;
using VoidMain.Text;

namespace VoidMain.IO.Console
{
    public class ConsoleStyledTextWriter : ConsoleTextWriter, IStyledTextWriter<TextStyle>
    {
        private readonly string _resetColor = "\x1b[0m";
        private readonly ReadOnlyMemory<char> _format;
        private readonly Memory<char> _setColor;
        private readonly Memory<char> _redSpan;
        private readonly Memory<char> _greenSpan;
        private readonly Memory<char> _blueSpan;

        public ConsoleStyledTextWriter(IConsole console)
            : base(console)
        {
            _setColor = "\x1b[38;2;000;000;000m".ToCharArray();
            _redSpan = _setColor.Slice(7, 3);
            _greenSpan = _setColor.Slice(11, 3);
            _blueSpan = _setColor.Slice(15, 3);
            _format = "D3".AsMemory();
        }

        public void ClearStyle() => _console.Write(_resetColor);

        public void WriteStyle(TextStyle style)
        {
            if (style.Background is null || style.Foreground is null)
            {
                ClearStyle();
            }

            var span = _setColor.Span;

            if (style.Foreground is Color foreground)
            {
                span[2] = '3'; // 38 is foreground
                FormatColor(foreground);
                _console.Write(span);
            }
            if (style.Background is Color background)
            {
                span[2] = '4'; // 48 is background
                FormatColor(background);
                _console.Write(span);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FormatColor(Color color)
        {
            var format = _format.Span;
            color.R.TryFormat(_redSpan.Span, out _, format);
            color.G.TryFormat(_greenSpan.Span, out _, format);
            color.B.TryFormat(_blueSpan.Span, out _, format);
        }
    }
}
