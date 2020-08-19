namespace VoidMain.Text.Templates.Parser
{
    public interface ITextTemplateParser<TPlaceholder, TStyle>
    {
        void Parse<TVisitor>(string templateText, ref TVisitor visitor)
            where TVisitor : ITextTemplateVisitor<TPlaceholder, TStyle>;
    }
}
