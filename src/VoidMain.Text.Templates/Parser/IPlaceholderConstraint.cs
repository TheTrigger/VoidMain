namespace VoidMain.Text.Templates.Parser
{
    public interface IPlaceholderConstraint
    {
        bool IsEndOfPlaceholder(string template, int position);
    }
}
