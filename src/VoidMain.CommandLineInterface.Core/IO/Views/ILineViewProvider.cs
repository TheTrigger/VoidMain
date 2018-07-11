namespace VoidMain.CommandLineInterface.IO.Views
{
    public interface ILineViewProvider
    {
        ILineView GetView(LineViewOptions options);
    }
}
