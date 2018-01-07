namespace VoidMain.CommandLineIinterface.IO.Views
{
    public static class CommandLineViewExtensions
    {
        public static void ReplaceWith(this ICommandLineView lineView, string value)
        {
            lineView.MoveTo(0);
            lineView.TypeOver(value);
            if (lineView.Position != lineView.Length)
            {
                lineView.Delete(lineView.Length - lineView.Position);
            }
        }

        public static CommandLineViewSnapshot TakeSnapshot(this ICommandLineView lineView)
        {
            return new CommandLineViewSnapshot(lineView);
        }
    }
}
