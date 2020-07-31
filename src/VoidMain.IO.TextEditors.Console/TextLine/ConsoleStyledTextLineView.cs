using System;
using System.Buffers;
using System.Collections.Generic;
using VoidMain.IO.Console;
using VoidMain.Text.Style;

namespace VoidMain.IO.TextEditors.TextLine
{
    public class ConsoleStyledTextLineView<TStyle> : ITextLineView, IEditingEventsListener, IDisposable
        where TStyle : IEquatable<TStyle>
    {
        private readonly IConsole _console;
        private readonly IConsoleStyleSetter<TStyle> _styleSetter;
        private readonly ITextColorizer<TStyle> _colorizer;
        private readonly ColorizerVisitor _visitor;
        private readonly ConsoleContinuousCursor _cursor;
        private readonly ArrayPoolTextLine _line;
        private int _maxPosition;
        private int _prevLength;
        private bool _hasChanges;
        private char[] _prevBuffer;
        private char[] _currBuffer;
        private List<Highlight> _prevHighlights;
        private List<Highlight> _currHighlights;

        public int Length => _line.Length;

        public int Position
        {
            get => _line.Position;
            set
            {
                int oldPos = Position;
                _line.Position = value;
                _cursor.Move(value - oldPos);
            }
        }

        public ReadOnlySpan<char> Span => _line.Span;

        public uint ContentVersion => _line.ContentVersion;

        public ConsoleStyledTextLineView(
            IConsole console,
            IConsoleStyleSetter<TStyle> styleSetter,
            ITextColorizer<TStyle> colorizer)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _styleSetter = styleSetter ?? throw new ArgumentNullException(nameof(styleSetter));
            _colorizer = colorizer ?? throw new ArgumentNullException(nameof(colorizer));
            _visitor = new ColorizerVisitor();
            _cursor = new ConsoleContinuousCursor(console);
            _line = new ArrayPoolTextLine(console.BufferWidth);
            _maxPosition = 0;
            _prevLength = 0;
            _hasChanges = false;

            _prevBuffer = Array.Empty<char>();
            _currBuffer = Array.Empty<char>();

            _prevHighlights = new List<Highlight>();
            _currHighlights = new List<Highlight>();
        }

        ~ConsoleStyledTextLineView() => Dispose();

        public void Dispose()
        {
            _line.Dispose();
            _prevHighlights.Clear();
            _currHighlights.Clear();
            Release(_prevBuffer);
            Release(_currBuffer);
        }

        private static void Release(char[] array)
        {
            if (array.Length != 0)
            {
                ArrayPool<char>.Shared.Return(array, clearArray: true);
            }
        }

        public override string ToString() => _line.ToString();

        public void Clear()
        {
            _cursor.Move(-Position);
            _line.Clear();
            _hasChanges = true;
        }

        public void Delete(int count)
        {
            if (count == 0) return;

            _line.Delete(count);
            _hasChanges = true;

            if (count < 0)
            {
                _cursor.Move(count);
            }
        }

        private void MoveCursorSafe(int offset)
        {
            int newPos = Position + offset;

            if (newPos <= _maxPosition)
            {
                _cursor.Move(offset);
                return;
            }

            int width = _console.BufferWidth;
            int left = _console.CursorLeft;

            _console.Write(' ', offset);

            if ((left + offset) % width == 0)
            {
                _console.Write(' ');
                _console.CursorLeft = 0;
            }

            _maxPosition += offset;
        }

        public void Type(char value)
        {
            MoveCursorSafe(1);
            _line.Type(value);
            _hasChanges = true;
        }

        public void Type(string value)
        {
            if (value == null) return;
            Type(value.AsSpan());
        }

        public void Type(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty) return;

            MoveCursorSafe(value.Length);
            _line.Type(value);
            _hasChanges = true;
        }

        public void TypeOver(char value)
        {
            MoveCursorSafe(1);
            _line.TypeOver(value);
            _hasChanges = true;
        }

        public void TypeOver(string value)
        {
            if (value == null) return;
            TypeOver(value.AsSpan());
        }

        public void TypeOver(ReadOnlySpan<char> value)
        {
            if (value.IsEmpty) return;

            MoveCursorSafe(value.Length);
            _line.TypeOver(value);
            _hasChanges = true;
        }

        public void OnModifying(bool isNextChangeAvailable)
        {
            /* no-op */
        }

        public void OnModified(bool isNextChangeAvailable)
        {
            if (!_hasChanges || isNextChangeAvailable) return;
            Render(incremental: true);
            _hasChanges = false;
        }

        private void Render(bool incremental)
        {
            ColorizeText();

            bool hasNewContent = GetHighlights(incremental,
                out int written, out List<Highlight>.Enumerator highlights);

            _cursor.Move(written - Position);

            int width = _console.BufferWidth;
            int left = _console.CursorLeft;

            if (hasNewContent)
            {
                do
                {
                    var highlight = highlights.Current;

                    _styleSetter.SetStyle(highlight.Style);
                    _console.Write(highlight.Text.Span);

                    int length = highlight.Text.Length;
                    written += length;
                    left += length;
                } while (highlights.MoveNext());

                _styleSetter.ClearStyle();
                highlights.Dispose();
            }

            int clear = _prevLength - written;
            _prevLength = written;

            if (clear > 0)
            {
                _console.Write(' ', clear);
                written += clear;
                left += clear;
            }

            if (left % width == 0)
            {
                _console.Write(' ');
                _console.CursorLeft = 0;
            }

            _cursor.Move(Position - written);

            (_prevBuffer, _currBuffer) = (_currBuffer, _prevBuffer);
            (_prevHighlights, _currHighlights) = (_currHighlights, _prevHighlights);
            _currHighlights.Clear();
        }

        private void ColorizeText()
        {
            var span = Span;

            if (_currBuffer.Length < span.Length)
            {
                Release(_currBuffer);
                _currBuffer = ArrayPool<char>.Shared.Rent(span.Length);
            }

            var buffer = _currBuffer.AsMemory(0, span.Length);
            span.CopyTo(buffer.Span);

            _visitor.Length = 0;
            _visitor.Highlights = _currHighlights;
            _colorizer.Colorize(buffer, _visitor);

            if (_visitor.Length != Length)
            {
                throw new Exception("The length of colorized text is not equal to the length of original text");
            }
        }

        private bool GetHighlights(bool incremental, out int written, out List<Highlight>.Enumerator highlights)
        {
            written = 0;

            if (!incremental)
            {
                highlights = _currHighlights.GetEnumerator();
                return highlights.MoveNext();
            }

            bool hasNewContent;
            var current = _currHighlights.GetEnumerator();
            var previous = _prevHighlights.GetEnumerator();

            while ((hasNewContent = current.MoveNext()) && previous.MoveNext())
            {
                var c = current.Current;
                var p = previous.Current;

                if (!c.Equals(p)) break;
                written += c.Text.Length;
            }

            previous.Dispose();

            if (hasNewContent)
            {
                highlights = current;
            }
            else
            {
                highlights = default;
                current.Dispose();
            }

            return hasNewContent;
        }

        private struct Highlight : IEquatable<Highlight>
        {
            private static readonly EqualityComparer<TStyle> Comparer = EqualityComparer<TStyle>.Default;

            public TStyle Style { get; set; }
            public ReadOnlyMemory<char> Text { get; set; }

            public bool Equals(Highlight other) => Comparer.Equals(Style, other.Style)
                && Text.Span.SequenceEqual(other.Text.Span);
        }

        private sealed class ColorizerVisitor : ITextColorizerVisitor<TStyle>
        {
            public int Length { get; set; }
            public List<Highlight> Highlights { get; set; } = default!;

            public void Visit(TStyle style, ReadOnlyMemory<char> text)
            {
                Length += text.Length;
                Highlights.Add(new Highlight
                {
                    Style = style,
                    Text = text
                });
            }
        }

        public void ClearState()
        {
            _line.ClearState();
            _maxPosition = 0;
            _prevLength = 0;
            _hasChanges = false;
            _prevHighlights.Clear();
        }

        public void SetState(ReadOnlySpan<char> text, int position)
        {
            _line.SetState(text, position);
            _maxPosition = text.Length;
            _prevLength = text.Length;
            _hasChanges = false;
            _prevHighlights.Clear();
        }

        public void Render()
        {
            Render(incremental: false);
        }

        public void EndLine()
        {
            Position = Length;
            ClearState();
            if (_console.CursorLeft > 0)
            {
                _console.WriteLine();
            }
        }
    }
}
