namespace VoidMain.IO.Console
{
    public class ConsoleIgnoreStyleSetter<TStyle> : IConsoleStyleSetter<TStyle>
    {
        public void ClearStyle() { /* no-op */ }

        public void SetStyle(TStyle style) { /* no-op */ }
    }
}
