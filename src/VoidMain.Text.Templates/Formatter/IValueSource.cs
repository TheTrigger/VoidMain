namespace VoidMain.Text.Templates.Formatter
{
    public interface IValueSource<TKey>
    {
        object GetValue(TKey key);
    }
}
