namespace VoidMain.Text.Templating.Parser
{
    public interface IPlaceholderConstraint
    {
        bool IsEndOfPlaceholder(string template, int position);
    }
}
