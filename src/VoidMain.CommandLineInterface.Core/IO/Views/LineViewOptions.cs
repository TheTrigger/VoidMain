namespace VoidMain.CommandLineIinterface.IO.Views
{
    public class LineViewOptions
    {
        public LineViewType ViewType { get; set; }
        public char MaskSymbol { get; set; }

        public static LineViewOptions Normal =>
            new LineViewOptions
            {
                ViewType = LineViewType.Normal,
            };

        public static LineViewOptions Masked(char maskSymbol)
        {
            return new LineViewOptions
            {
                ViewType = LineViewType.Masked,
                MaskSymbol = maskSymbol
            };
        }

        public static LineViewOptions Hidden =>
            new LineViewOptions
            {
                ViewType = LineViewType.Hidden,
            };
    }
}
