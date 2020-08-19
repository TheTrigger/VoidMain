using System;
using VoidMain.Text.Style;

namespace VoidMain.IO.Console
{
    public class ConsoleTextWriter<TStyle> : ConsoleTextWriter, ITextWriter<TStyle>
    {
        private readonly IConsoleStyleSetter<TStyle> _styleSetter;

        public ConsoleTextWriter(IConsole console, IConsoleStyleSetter<TStyle> styleSetter)
                : base(console)
        {
            _styleSetter = styleSetter ?? throw new ArgumentNullException(nameof(styleSetter));
        }

        public void ClearStyle() => _styleSetter.ClearStyle();

        public void SetStyle(TStyle style) => _styleSetter.SetStyle(style);
    }
}
