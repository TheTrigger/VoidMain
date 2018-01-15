namespace VoidMain.Application.Commands.Arguments
{
    public interface ICollectionInitializer
    {
        object Collection { get; }
        void SetValue(int index, object value);
    }
}
