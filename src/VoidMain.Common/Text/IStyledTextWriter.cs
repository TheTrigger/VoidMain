namespace VoidMain.Text
{
    public interface IStyledTextWriter<TStyle> : ITextWriter
    // TODO: where TStyle : notnull ???
    {
        void WriteStyle(TStyle style);
        void ClearStyle();
    }
}
