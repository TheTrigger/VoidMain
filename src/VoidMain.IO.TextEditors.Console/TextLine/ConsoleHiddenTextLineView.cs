using System;
using VoidMain.IO.Console;

namespace VoidMain.IO.TextEditors.TextLine
{
    public class ConsoleHiddenTextLineView : ITextLineView, IDisposable
    {
        private readonly IConsole _console;
        private readonly ArrayPoolTextLine _line;

        public int Length => _line.Length;

        public int Position
        {
            get => _line.Position;
            set => _line.Position = value;
        }

        public ReadOnlySpan<char> Span => _line.Span;

        public uint ContentVersion => _line.ContentVersion;

        public ConsoleHiddenTextLineView(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _line = new ArrayPoolTextLine(console.BufferWidth);
        }

        ~ConsoleHiddenTextLineView() => Dispose();

        public void Dispose() => _line.Dispose();

        public override string ToString() => _line.ToString();

        public void Clear() => _line.Clear();

        public void Delete(int count) => _line.Delete(count);

        public void Type(char value) => _line.Type(value);

        public void Type(string value) => _line.Type(value);

        public void Type(ReadOnlySpan<char> value) => _line.Type(value);

        public void TypeOver(char value) => _line.TypeOver(value);

        public void TypeOver(string value) => _line.TypeOver(value);

        public void TypeOver(ReadOnlySpan<char> value) => _line.TypeOver(value);

        public void ClearState() => _line.ClearState();

        public void SetState(ReadOnlySpan<char> text, int position)
            => _line.SetState(text, position);

        public void Render() { }

        public void EndLine()
        {
            ClearState();
            if (_console.CursorLeft > 0)
            {
                _console.WriteLine();
            }
        }
    }
}
