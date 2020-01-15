namespace VoidMain.Text.Templating.Parser
{
    public interface ITextTemplateParser<TPlaceholder>
    {
        void Parse<TVisitor>(string templateText, TVisitor visitor)
            where TVisitor : ITextTemplateVisitor<TPlaceholder>;
    }
}
