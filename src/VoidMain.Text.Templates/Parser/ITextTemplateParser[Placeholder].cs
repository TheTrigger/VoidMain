namespace VoidMain.Text.Templates.Parser
{
    public interface ITextTemplateParser<TPlaceholder>
    {
        void Parse<TVisitor>(string templateText, ref TVisitor visitor)
            where TVisitor : ITextTemplateVisitor<TPlaceholder>;
    }
}
