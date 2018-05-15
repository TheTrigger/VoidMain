using System;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleHighlightedLineView : ILineView, ILineViewInputLifecycle
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly ITextHighlighter<ConsoleTextStyle> _textHighlighter;
        private readonly InMemoryLineView _line;
        private ConsoleColor _background;
        private ConsoleColor _foreground;
        private bool _hasChanges;
        private int _prevLength;

        public LineViewType ViewType { get; }
        public int Position => _line.Position;
        public int Length => _line.Length;
        public char this[int index] => _line[index];

        public ConsoleHighlightedLineView(
            IConsole console, IConsoleCursor cursor,
            ITextHighlighter<ConsoleTextStyle> textHighlighter)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _textHighlighter = textHighlighter ?? throw new ArgumentNullException(nameof(textHighlighter));
            _line = new InMemoryLineView();
            ViewType = LineViewType.Normal;
            _hasChanges = false;
            _prevLength = 0;
        }

        public string ToString(int start, int length) => _line.ToString(start, length);
        public string ToString(int start) => _line.ToString(start);
        public override string ToString() => _line.ToString();

        #region View Operations

        public void Move(int offset)
        {
            _line.Move(offset);
            _cursor.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            int oldPos = Position;
            _line.MoveTo(newPos);
            _cursor.Move(newPos - oldPos);
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            _line.Delete(count);
            if (count < 0)
            {
                _cursor.Move(count);
            }

            _hasChanges = true;
        }

        public void Clear()
        {
            _cursor.Move(-Position);
            _line.Clear();
            _hasChanges = true;
        }

        public void Type(char value)
        {
            _cursor.Move(1);
            _line.Type(value);
            _hasChanges = true;
        }

        public void TypeOver(char value)
        {
            _cursor.Move(1);
            _line.TypeOver(value);
            _hasChanges = true;
        }

        public void Type(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            _cursor.Move(value.Length);
            _line.Type(value);
            _hasChanges = true;
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            _cursor.Move(value.Length);
            _line.TypeOver(value);
            _hasChanges = true;
        }

        #endregion

        #region Rendering

        private void RenderLine()
        {
            string commandLine = ToString();
            var highlights = _textHighlighter.Highlight(commandLine);

            int pos = Position;
            int written = 0;
            _cursor.Move(-pos);

            foreach (var highlight in highlights)
            {
                var style = highlight.Style ?? ConsoleTextStyle.Default;
                var span = highlight.Span;

                if (span.Start != written)
                {
                    throw new IndexOutOfRangeException();
                }

                ApplyStyle(style);
                _console.Write(span.Text);
                written += span.Length;
            }

            int clearSpace = WriteBlank(_prevLength - written);
            _prevLength = written;
            _cursor.Move(pos - written - clearSpace);
        }

        private int WriteBlank(int length)
        {
            if (length < 1) return 0;
            _console.ForegroundColor = _foreground;
            _console.BackgroundColor = _background;
            _console.Write(' ', length);
            return length;
        }

        private void ApplyStyle(ConsoleTextStyle style)
        {
            _console.ForegroundColor = style.Foreground ?? _foreground;
            _console.BackgroundColor = style.Background ?? _background;
        }

        #endregion

        #region Input Lifecycle

        public void BeforeLineReading()
        {
            _foreground = _console.ForegroundColor;
            _background = _console.BackgroundColor;
        }

        public void AfterLineReading()
        {
            _console.ForegroundColor = _foreground;
            _console.BackgroundColor = _background;
        }

        public void BeforeInputHandling(bool isNextKeyAvailable)
        {
        }

        public void AfterInputHandling(bool isNextKeyAvailable)
        {
            if (!_hasChanges || isNextKeyAvailable) return;
            RenderLine();
            _hasChanges = false;
        }

        #endregion
    }
}
