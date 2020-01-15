namespace VoidMain.Text.Templating.Parser
{
    public interface IPlaceholderParser<TPlaceholder>
    {
        int Parse<TPlaceholderConstraint>(string template, int position,
            TPlaceholderConstraint placeholderConstraint, out TPlaceholder placeholder)
            where TPlaceholderConstraint : struct, IPlaceholderConstraint;
    }
}
