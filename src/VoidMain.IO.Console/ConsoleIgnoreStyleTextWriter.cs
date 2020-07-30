using VoidMain.Text.Style;

namespace VoidMain.IO.Console
{
    public class ConsoleIgnoreStyleTextWriter<TStyle> : ConsoleTextWriter, IStyledTextWriter<TStyle>
    {
        public ConsoleIgnoreStyleTextWriter(IConsole console)
            : base(console) { }

        public void ClearStyle() { /* no-op */ }

        public void WriteStyle(TStyle style) { /* no-op */ }
    }
}
