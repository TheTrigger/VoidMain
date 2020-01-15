using System;

namespace VoidMain.Text.Templating
{
    public interface ITextTemplateVisitor<TPlaceholder>
    {
        void BeforeVisitAll();
        void AfterVisitAll();
        void Visit(ReadOnlyMemory<char> text);
        void Visit(TPlaceholder placeholder);
    }
}
