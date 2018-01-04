using System;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.CommandLineIinterface.SyntaxHighlight;

namespace VoidMain.CommandLineIinterface.Console.Views
{
    public class ConsoleCommandLineHighlightedView : ICommandLineView, ICommandLineViewLifecycle
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _cursor;
        private readonly ICommandLineParser _parser;
        private readonly ISyntaxHighlighter<ConsoleColor?> _highlighter;
        private readonly SyntaxPallete<ConsoleColor?> _pallete;
        private readonly CommandLineBuilder _lineBuilder;
        private ConsoleColor _background;
        private ConsoleColor _foreground;
        private bool _hasChanges;
        private int _prevLength;

        public ConsoleCommandLineHighlightedView(
            IConsole console, IConsoleCursor cursor, ICommandLineParser parser,
            ISyntaxHighlighter<ConsoleColor?> highlighter, SyntaxPallete<ConsoleColor?> pallete)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _cursor = cursor ?? throw new ArgumentNullException(nameof(cursor));
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _highlighter = highlighter ?? throw new ArgumentNullException(nameof(highlighter));
            _pallete = pallete ?? throw new ArgumentNullException(nameof(pallete));
            _lineBuilder = new CommandLineBuilder();
            ViewType = CommandLineViewType.Normal;
            MaskSymbol = Char.MinValue;
            _hasChanges = false;
            _prevLength = 0;
        }

        public CommandLineViewType ViewType { get; private set; }
        public char MaskSymbol { get; private set; }

        public int Position => _lineBuilder.Position;
        public int Length => _lineBuilder.Length;
        public char this[int index] => _lineBuilder[index];

        public string ToString(int start, int length) => _lineBuilder.ToString(start, length);
        public string ToString(int start) => _lineBuilder.ToString(start);
        public override string ToString() => _lineBuilder.ToString();

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

        public void ClearAll()
        {
            _cursor.Move(-_lineBuilder.Length);
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

        private void Render()
        {
            string commandLine = ToString();
            var syntax = _parser.Parse(commandLine);
            var coloredSpans = _highlighter.GetColoredSpans(syntax, _pallete);

            int pos = Position;
            int written = 0;
            _cursor.Move(-pos);

            foreach (var coloredSpan in coloredSpans)
            {
                var span = coloredSpan.Span;
                int gap = span.Start - written;

                if (gap > 0)
                {
                    _console.ResetColors();
                    _console.Write(' ', gap);
                    written += gap;
                }

                _console.BackgroundColor = coloredSpan.Background ?? _background;
                _console.ForegroundColor = coloredSpan.Foreground ?? _foreground;
                _console.Write(span.Text);
                written += span.Length;
            }

            if (written < _prevLength)
            {
                _console.ResetColors();
                int clearSpace = _prevLength - written;
                _console.Write(' ', clearSpace);
                written += clearSpace;
            }

            _cursor.Move(pos - written);
            _prevLength = commandLine.Length;
        }

        #region View Lifecycle

        public void BeginReadingLine()
        {
            _background = _console.BackgroundColor;
            _foreground = _console.ForegroundColor;
        }

        public void EndReadingLine()
        {
            _console.BackgroundColor = _background;
            _console.ForegroundColor = _foreground;
        }

        public void BeginHandlingInput()
        {
            _hasChanges = false;
        }

        public void EndHandlingInput()
        {
            if (_hasChanges) Render();
        }

        #endregion
    }
}
