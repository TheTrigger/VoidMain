using System;

namespace VoidMain.Text.Style
{
    public interface ITextColorizer<TStyle>
    {
        void Colorize<TVisitor>(ReadOnlyMemory<char> text, TVisitor visitor)
            where TVisitor : ITextColorizerVisitor<TStyle>;
    }
}
