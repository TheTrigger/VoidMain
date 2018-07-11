namespace VoidMain.CommandLineInterface.IO.Views
{
    public static class LineViewExtensions
    {
        public static void ReplaceWith(this ILineView lineView, string value)
        {
            lineView.MoveTo(0);
            lineView.TypeOver(value);
            if (lineView.Position != lineView.Length)
            {
                lineView.Delete(lineView.Length - lineView.Position);
            }
        }

        public static LineViewSnapshot TakeSnapshot(this ILineView lineView)
        {
            return new LineViewSnapshot(lineView);
        }
    }
}
