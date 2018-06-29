namespace VoidMain.CommandLineIinterface.IO.Views
{
    public interface IReusableLineView
    {
        void SetState(string line, int position);
        void ClearState();
    }
}
