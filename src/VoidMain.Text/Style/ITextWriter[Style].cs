namespace VoidMain.Text.Style
{
    public interface ITextWriter<TStyle> : ITextWriter
    {
        void ClearStyle();
        void SetStyle(TStyle style);
    }
}
