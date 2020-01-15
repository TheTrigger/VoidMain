using System;

namespace VoidMain.Text.Templating
{
    public interface ITextTemplateVisitor<TPlaceholder, TStyle>
    {
        void BeforeVisitAll();
        void AfterVisitAll();
        void Visit(ReadOnlyMemory<char> text, TStyle style);
        void Visit(TPlaceholder placeholder, TStyle style);
    }
}
