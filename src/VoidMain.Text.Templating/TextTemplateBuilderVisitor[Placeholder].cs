using System;

namespace VoidMain.Text.Templating
{
    public struct TextTemplateBuilderVisitor<TPlaceholder> : ITextTemplateVisitor<TPlaceholder>
    {
        public TextTemplate<TPlaceholder> Template { get; }

        public TextTemplateBuilderVisitor(TextTemplate<TPlaceholder> template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public void BeforeVisitAll() { }

        public void AfterVisitAll() { }

        public void Visit(ReadOnlyMemory<char> text)
        {
            Template.Add(new TextTemplate<TPlaceholder>.TextToken(text));
        }

        public void Visit(TPlaceholder placeholder)
        {
            Template.Add(new TextTemplate<TPlaceholder>.PlaceholderToken(placeholder));
        }
    }
}
