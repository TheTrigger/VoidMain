namespace VoidMain.Text.Style
{
    public class SingleStyleTextColorizerOptions<TStyle>
    {
        public TStyle Style { get; set; } = default!;
        public int SplitSpanByLength { get; set; } = 10;
    }
}
