namespace VoidMain.CommandLineInterface.IO.Views
{
    public interface IReusableLineView
    {
        void SetState(string line, int position);
        void ClearState();
        void RenderState();
    }
}
