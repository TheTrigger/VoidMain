namespace VoidMain.Text.Style
{
    public interface IStyledTextWriter<TStyle> : ITextWriter
    {
        void ClearStyle();
        void SetStyle(TStyle style);
    }
}
