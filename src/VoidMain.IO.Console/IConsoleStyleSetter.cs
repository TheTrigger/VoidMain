namespace VoidMain.IO.Console
{
    public interface IConsoleStyleSetter<TStyle>
    {
        void ClearStyle();
        void SetStyle(TStyle style);
    }
}
