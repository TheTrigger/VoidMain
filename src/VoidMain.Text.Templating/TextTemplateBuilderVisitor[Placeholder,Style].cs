using System;

namespace VoidMain.Text.Templating
{
    public struct TextTemplateBuilderVisitor<TPlaceholder, TStyle> : ITextTemplateVisitor<TPlaceholder, TStyle>
    {
        public TextTemplate<TPlaceholder, TStyle> Template { get; }

        public TextTemplateBuilderVisitor(TextTemplate<TPlaceholder, TStyle> template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public void BeforeVisitAll() { }

        public void AfterVisitAll() { }

        public void Visit(ReadOnlyMemory<char> text, TStyle style)
        {
            Template.Add(new TextTemplate<TPlaceholder, TStyle>.TextToken(text, style));
        }

        public void Visit(TPlaceholder placeholder, TStyle style)
        {
            Template.Add(new TextTemplate<TPlaceholder, TStyle>.PlaceholderToken(placeholder, style));
        }
    }
}
