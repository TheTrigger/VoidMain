using System;

namespace VoidMain.Text.Templates
{
    public interface ITextTemplateVisitor<TPlaceholder>
    {
        void BeforeVisitAll();
        void AfterVisitAll();
        void Visit(ReadOnlyMemory<char> text);
        void Visit(TPlaceholder placeholder);
    }
}
