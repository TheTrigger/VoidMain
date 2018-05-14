namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface ILineViewProvider
    {
        ILineView GetView(LineViewOptions options);
    }
}
