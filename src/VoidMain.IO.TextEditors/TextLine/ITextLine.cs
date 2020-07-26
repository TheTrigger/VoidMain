using System;

namespace VoidMain.IO.TextEditors.TextLine
{
    public interface ITextLine
    {
        int Length { get; }
        int Position { get; set; }
        ReadOnlySpan<char> Span { get; }
        uint ContentVersion { get; }

        void Type(char value);
        void Type(string value);
        void Type(ReadOnlySpan<char> value);

        void TypeOver(char value);
        void TypeOver(string value);
        void TypeOver(ReadOnlySpan<char> value);

        void Delete(int count);
        void Clear();
    }
}
