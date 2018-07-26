using System;
using System.Collections.Generic;
using VoidMain.CommandLineInterface.Highlighting;
using VoidMain.CommandLineInterface.IO.Console;

namespace VoidMain.CommandLineInterface.IO.Views
{
    public class ConsoleHighlightedLineView : ILineView, IReusableLineView, ILineViewInputLifecycle
    {
        private readonly IConsole _console;
        private readonly IConsoleCursor _consoleCursor;
        private readonly IColoredTextWriter _coloredTextWriter;
        private readonly ITextHighlighter<TextStyle> _textHighlighter;
        private IReadOnlyList<StyledSpan<TextStyle>> _prevHighlights;
        private readonly InMemoryLineView _line;
        private bool _hasChanges;
        private int _prevLength;
        private int _maxPosition;

        public LineViewType ViewType { get; }
        public int Position => _line.Position;
        public int Length => _line.Length;
        public char this[int index] => _line[index];

        public ConsoleHighlightedLineView(
            IConsole console, IConsoleCursor consoleCursor,
            IColoredTextWriter coloredTextWriter,
            ITextHighlighter<TextStyle> textHighlighter)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _consoleCursor = consoleCursor ?? throw new ArgumentNullException(nameof(consoleCursor));
            _coloredTextWriter = coloredTextWriter ?? throw new ArgumentNullException(nameof(coloredTextWriter));
            _textHighlighter = textHighlighter ?? throw new ArgumentNullException(nameof(textHighlighter));
            _prevHighlights = Array.Empty<StyledSpan<TextStyle>>();
            _line = new InMemoryLineView();
            ViewType = LineViewType.Normal;
            _hasChanges = false;
            _prevLength = 0;
            _maxPosition = 0;
        }

        public string ToString(int start, int length) => _line.ToString(start, length);
        public string ToString(int start) => _line.ToString(start);
        public override string ToString() => _line.ToString();

        #region View Operations

        public void Move(int offset)
        {
            _line.Move(offset);
            _consoleCursor.Move(offset);
        }

        public void MoveTo(int newPos)
        {
            int oldPos = Position;
            _line.MoveTo(newPos);
            _consoleCursor.Move(newPos - oldPos);
        }

        /// <summary>
        /// Prevents ArgumentOutOfRangeException when reaching BufferHeight.
        /// </summary>
        private void MoveCursorSafe(int offset)
        {
            int newPos = Position + offset;

            if (newPos <= _maxPosition)
            {
                _consoleCursor.Move(offset);
            }
            else
            {
                int available = _maxPosition - Position;
                _consoleCursor.Move(available);

                int fill = offset - available;
                _console.Write(' ', fill);
                _maxPosition += fill;
            }
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            _line.Delete(count);
            if (count < 0)
            {
                _consoleCursor.Move(count);
            }

            _hasChanges = true;
        }

        public void Clear()
        {
            _consoleCursor.Move(-Position);
            _line.Clear();
            _hasChanges = true;
        }

        public void Type(char value)
        {
            MoveCursorSafe(1);
            _line.Type(value);
            _hasChanges = true;
        }

        public void TypeOver(char value)
        {
            MoveCursorSafe(1);
            _line.TypeOver(value);
            _hasChanges = true;
        }

        public void Type(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            MoveCursorSafe(value.Length);
            _line.Type(value);
            _hasChanges = true;
        }

        public void TypeOver(string value)
        {
            if (String.IsNullOrEmpty(value)) return;

            MoveCursorSafe(value.Length);
            _line.TypeOver(value);
            _hasChanges = true;
        }

        #endregion

        #region Reusable View

        public void SetState(string line, int position)
        {
            _line.SetState(line, position);
            _prevHighlights = _textHighlighter.Highlight(line);
            _hasChanges = false;
            _prevLength = line.Length;
            _maxPosition = line.Length;
        }

        public void ClearState()
        {
            _line.ClearState();
            _prevHighlights = Array.Empty<StyledSpan<TextStyle>>();
            _hasChanges = false;
            _prevLength = 0;
            _maxPosition = 0;
        }

        public void RenderState()
        {
            _prevLength = 0;
            _prevHighlights = Array.Empty<StyledSpan<TextStyle>>();
            RenderLine();
        }

        #endregion

        #region Input Lifecycle

        public void BeforeLineReading()
        {
            // do nothing
        }

        public void AfterLineReading()
        {
            // do nothing
        }

        public void BeforeInputHandling(bool hasMoreInput)
        {
            // do nothing
        }

        public void AfterInputHandling(bool hasMoreInput)
        {
            if (!_hasChanges || hasMoreInput) return;
            RenderLine();
            _hasChanges = false;
        }

        #endregion

        #region Rendering

        private void RenderLine()
        {
            int written = 0;
            string line = ToString();
            var highlights = _textHighlighter.Highlight(line);

            int lastUnchanged = IndexOfLastUnchanged(_prevHighlights, highlights);
            if (lastUnchanged >= 0)
            {
                written = highlights[lastUnchanged].Span.End;
            }

            _consoleCursor.Move(written - Position);

            for (int i = lastUnchanged + 1; i < highlights.Count; i++)
            {
                var highlight = highlights[i];
                var style = highlight.Style;
                var span = highlight.Span;

                if (span.Start != written)
                {
                    throw new IndexOutOfRangeException(); // TODO: add error message
                }

                _coloredTextWriter.Write(style?.Foreground, style?.Background, span.Text);
                written += span.Length;
            }

            if (written != line.Length)
            {
                throw new IndexOutOfRangeException(); // TODO: add error message
            }

            int clearSpace = WriteBlank(_prevLength - written);
            _consoleCursor.Move(Position - written - clearSpace);

            _prevLength = written;
            _prevHighlights = highlights;
        }

        private int IndexOfLastUnchanged(
           IReadOnlyList<StyledSpan<TextStyle>> previous,
           IReadOnlyList<StyledSpan<TextStyle>> current)
        {
            int index = 0;
            while (index < previous.Count && index < current.Count)
            {
                bool equals = previous[index].Span == current[index].Span
                    && previous[index].Style == current[index].Style;
                if (!equals) break;
                index++;
            }
            return index - 1;
        }

        private int WriteBlank(int length)
        {
            if (length < 1) return 0;
            _coloredTextWriter.Write(null, null, ' ', length);
            return length;
        }

        #endregion
    }
}
