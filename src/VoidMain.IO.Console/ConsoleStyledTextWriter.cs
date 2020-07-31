using System;
using VoidMain.Text.Style;

namespace VoidMain.IO.Console
{
    public class ConsoleStyledTextWriter<TStyle> : ConsoleTextWriter, IStyledTextWriter<TStyle>
    {
        private readonly IConsoleStyleSetter<TStyle> _styleSetter;

        public ConsoleStyledTextWriter(IConsole console, IConsoleStyleSetter<TStyle> styleSetter)
                : base(console)
        {
            _styleSetter = styleSetter ?? throw new ArgumentNullException(nameof(styleSetter));
        }

        public void ClearStyle() => _styleSetter.ClearStyle();

        public void SetStyle(TStyle style) => _styleSetter.SetStyle(style);
    }
}
