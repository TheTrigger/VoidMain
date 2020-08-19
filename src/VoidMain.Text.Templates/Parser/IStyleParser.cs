namespace VoidMain.Text.Templates.Parser
{
    public interface IStyleParser<TStyle>
    {
        int Parse<TParseRange>(
            string template, int position,
            TParseRange range, out TStyle style)
            where TParseRange : struct, IParseRange;
    }
}
