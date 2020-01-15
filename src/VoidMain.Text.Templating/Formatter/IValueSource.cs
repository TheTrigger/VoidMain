namespace VoidMain.Text.Templating.Formatter
{
    public interface IValueSource<TKey>
    {
        object GetValue(TKey key);
    }
}
