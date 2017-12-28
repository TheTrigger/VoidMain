namespace VoidMain.CommandLineIinterface
{
    public static class CommandLineViewExtensions
    {
        public static void ReplaceWith(this ICommandLineView view, string value)
        {
            view.MoveTo(0);
            view.TypeOver(value);
            if (view.Position != view.Length)
            {
                view.Delete(view.Length - view.Position);
            }
        }
    }
}
