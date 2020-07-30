using System;

namespace VoidMain.Text.Style
{
    public interface ITextColorizerVisitor<TStyle>
    {
        public void Visit(TStyle style, ReadOnlyMemory<char> text);
    }
}
