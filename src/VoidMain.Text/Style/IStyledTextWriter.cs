namespace VoidMain.Text.Style
{
    public interface IStyledTextWriter<TStyle> : ITextWriter
    // TODO: where TStyle : notnull ???
    {
        void WriteStyle(TStyle style);
        void ClearStyle();
    }
}
