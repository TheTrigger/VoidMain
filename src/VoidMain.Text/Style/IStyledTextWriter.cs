namespace VoidMain.Text.Style
{
    public interface IStyledTextWriter<TStyle> : ITextWriter
    {
        void WriteStyle(TStyle style);
        void ClearStyle();
    }
}
