using System;
using VoidMain.CommandLineIinterface.Internal;
using VoidMain.CommandLineIinterface.IO.Console;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class ConsoleHighlightedLineView : ILineView, ILineViewInputLifecycle
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly ICommandLineParser _parser;
        private readonly ISyntaxHighlighter<ConsoleTextStyle> _highlighter;
        private readonly SyntaxHighlightingPallete<ConsoleTextStyle> _pallete;
        private readonly CommandLineBuilder _lineBuilder;
        private ConsoleColor _background;
        private ConsoleColor _foreground;
        private bool _hasChanges;
        private int _prevLength;

        public ConsoleHighlightedLineView(
            IConsole console, IConsoleCursor cursor, ICommandLineParser parser,
            ISyntaxHighlighter<ConsoleTextStyle> highlighter, SyntaxHighlightingPallete<ConsoleTextStyle> pallete)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _highlighter = highlighter ?? throw new ArgumentNullException(nameof(highlighter));
            _pallete = pallete ?? throw new ArgumentNullException(nameof(pallete));
            _lineBuilder = new CommandLineBuilder();
            ViewType = LineViewType.Normal;
            _hasChanges = false;
            _prevLength = 0;
        }

        public LineViewType ViewType { get; }
        public int Position => _lineBuilder.Position;
        public int Length => _lineBuilder.Length;
        public char this[int index] => _lineBuilder[index];

        public string ToString(int start, int length) => _lineBuilder.ToString(start, length);
        public string ToString(int start) => _lineBuilder.ToString(start);
        public override string ToString() => _lineBuilder.ToString();

        #region View Operations

        public void Move(int offset)
        {
            // Throws if out of range
            _lineBuilder.Move(offset);
            _cursor.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            int oldPos = _lineBuilder.Position;
            // Throws if out of range
            _lineBuilder.MoveTo(newPos);
            _cursor.Move(newPos - oldPos);
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            // Throws if out of range
            _lineBuilder.Delete(count);
            if (count < 0)
            {
                _cursor.Move(count);
            }

            _hasChanges = true;
        }

        public void Clear()
        {
            _cursor.Move(-_lineBuilder.Position);
            _lineBuilder.Clear();
            _hasChanges = true;
        }

        public void Type(char value)
        {
            _lineBuilder.Insert(value);
            _cursor.Move(1);
            _hasChanges = true;
        }

        public void TypeOver(char value)
        {
            if (_lineBuilder.Position < _lineBuilder.Length)
            {
                _lineBuilder[_lineBuilder.Position] = value;
                _lineBuilder.Move(1);
            }
            else
            {
                _lineBuilder.Insert(value);
            }
            _cursor.Move(1);
            _hasChanges = true;
        }

        public void Type(string value)
        {
            if (String.IsNullOrEmpty(value)) return;
            _lineBuilder.Insert(value);
            _cursor.Move(value.Length);
            _hasChanges = true;
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            int offset = 0;
            while (_lineBuilder.Position < _lineBuilder.Length && offset < value.Length)
            {
                _lineBuilder[_lineBuilder.Position] = value[offset];
                _lineBuilder.Move(1);
                offset++;
            }
            if (offset < value.Length)
            {
                _lineBuilder.Insert(value.Substring(offset));
            }

            _cursor.Move(value.Length);
            _hasChanges = true;
        }

        #endregion

        #region Rendering

        private void RenderHighlightedLine()
        {
            string commandLine = ToString();
            var syntax = _parser.Parse(commandLine);
            var highlightedSpans = _highlighter.GetHighlightedSpans(syntax, _pallete);

            int pos = Position;
            int written = 0;
            _cursor.Move(-pos);

            foreach (var highlightedSpan in highlightedSpans)
            {
                var span = highlightedSpan.Span;
                var style = highlightedSpan.Style ?? ConsoleTextStyle.Default;

                int spansGap = WriteBlank(span.Start - written);
                written += spansGap;

                ApplyStyle(style);
                _console.Write(span.Text);
                written += span.Length;
            }

            int clearSpace = WriteBlank(_prevLength - written);
            written += clearSpace;

            _cursor.Move(pos - written);
            _prevLength = commandLine.Length;
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

        #region View Lifecycle

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
            RenderHighlightedLine();
            _hasChanges = false;
        }

        #endregion
    }
}
