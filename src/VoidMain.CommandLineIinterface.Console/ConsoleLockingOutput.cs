using System;
using VoidMain.CommandLineIinterface.IO;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface
{
    public class ConsoleLockingOutput : ICommandLineOutput
    {
        private readonly Type CustomFormatterType = typeof(ICustomFormatter);

        private readonly IConsole _console;
        private readonly IConsoleColorConverter _colorConverter;
        private readonly IMessageTemplateParser _templateParser;
        private readonly ConsoleOutputLock _lock;

        public ConsoleLockingOutput(
            IConsole console,
            IConsoleColorConverter colorConverter,
            IMessageTemplateParser templateParser,
            ConsoleOutputLock @lock)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _colorConverter = colorConverter ?? throw new ArgumentNullException(nameof(colorConverter));
            _templateParser = templateParser ?? throw new ArgumentNullException(nameof(templateParser));
            _lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
        }

        public void Write(char value)
        {
            _lock.ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(string value)
        {
            _lock.ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(object value)
        {
            _lock.ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(string format, params object[] args)
        {
            Write(null, format, args);
        }

        public void Write(IFormatProvider formatProvider, string format, params object[] args)
        {
            _lock.ThrowIfLocked();

            var parsedTemplate = _templateParser.Parse(format);
            var formatter = GetFormatter(formatProvider);

            foreach (var token in parsedTemplate.Tokens)
            {
                switch (token)
                {
                    case MessageTemplate.TextToken text:
                        _console.Write(text.Text);
                        break;
                    case MessageTemplate.ArgumentToken arg:
                        var value = args[arg.Index];
                        string formatedValue = FormatValue(formatProvider, formatter, value, arg.Format);
                        if (arg.Alignment > 0)
                        {
                            int padRight = arg.Alignment - formatedValue.Length;
                            if (padRight > 0)
                            {
                                _console.Write(' ', padRight);
                            }
                        }

                        _console.Write(formatedValue);

                        if (arg.Alignment < 0)
                        {
                            int padLeft = -arg.Alignment - formatedValue.Length;
                            if (padLeft > 0)
                            {
                                _console.Write(' ', padLeft);
                            }
                        }
                        break;
                    default:
                        throw new FormatException($"Unknown format token `{token.GetType().Name}`.");
                }
            }
        }

        public void WriteLine()
        {
            _lock.ThrowIfLocked();
            _console.WriteLine();
        }

        public void WriteLine(string value)
        {
            _lock.ThrowIfLocked();
            _console.WriteLine(value);
        }

        public void WriteLine(object value)
        {
            _lock.ThrowIfLocked();
            _console.WriteLine(value);
        }

        public void WriteLine(string format, params object[] args)
        {
            Write(null, format, args);
            _console.WriteLine();
        }

        public void WriteLine(IFormatProvider formatProvider, string format, params object[] args)
        {
            Write(formatProvider, format, args);
            _console.WriteLine();
        }

        public void Write(Color foreground, char value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, null);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void Write(Color foreground, string value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, null);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void Write(Color foreground, object value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, null);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void WriteLine(Color foreground, string value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, null);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void WriteLine(Color foreground, object value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, null);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void Write(Color foreground, Color background, char value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, background);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void Write(Color foreground, Color background, string value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, background);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void Write(Color foreground, Color background, object value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, background);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void WriteLine(Color foreground, Color background, string value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, background);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void WriteLine(Color foreground, Color background, object value)
        {
            _lock.ThrowIfLocked();
            GetConsoleColors(out var fg, out var bg);
            SetColors(foreground, background);
            _console.WriteLine(value);
            SetConsoleColors(fg, bg);
        }

        public void Write(ColoredFormat format)
        {
            _lock.ThrowIfLocked();

            var parsedTemplate = _templateParser.Parse(format.Template.Value);
            var formatter = GetFormatter(format.FormatProvider);

            GetConsoleColors(out var defaultFg, out var defaultBg);

            var templateFg = GetColor(format.Template.Foreground, defaultFg);
            var templateBg = GetColor(format.Template.Background, defaultBg);

            foreach (var token in parsedTemplate.Tokens)
            {
                switch (token)
                {
                    case MessageTemplate.TextToken text:
                        SetConsoleColors(templateFg, templateBg);
                        _console.Write(text.Text);
                        break;
                    case MessageTemplate.ArgumentToken arg:
                        var coloredValue = format[arg.Index];
                        string formatedValue = FormatValue(format.FormatProvider, formatter, coloredValue.Value, arg.Format);

                        if (arg.Alignment > 0)
                        {
                            int padRight = arg.Alignment - formatedValue.Length;
                            if (padRight > 0)
                            {
                                SetConsoleColors(templateFg, templateBg);
                                _console.Write(' ', padRight);
                            }
                        }

                        var argFg = GetColor(coloredValue.Foreground, templateFg);
                        var argBg = GetColor(coloredValue.Background, templateBg);

                        SetConsoleColors(argFg, argBg);
                        _console.Write(formatedValue);

                        if (arg.Alignment < 0)
                        {
                            int padLeft = -arg.Alignment - formatedValue.Length;
                            if (padLeft > 0)
                            {
                                SetConsoleColors(templateFg, templateBg);
                                _console.Write(' ', padLeft);
                            }
                        }
                        break;
                    default:
                        throw new FormatException($"Unknown format token `{token.GetType().Name}`.");
                }
            }

            SetConsoleColors(defaultFg, defaultBg);
        }

        public void WriteLine(ColoredFormat format)
        {
            Write(format);
            _console.WriteLine();
        }

        private ICustomFormatter GetFormatter(IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                return null;
            }
            return (ICustomFormatter)formatProvider.GetFormat(CustomFormatterType);
        }

        private string FormatValue(IFormatProvider formatProvider, ICustomFormatter formatter, object value, string format)
        {
            string result = null;

            if (formatter != null)
            {
                result = formatter.Format(format, value, formatProvider);
            }

            if (result == null)
            {
                var formattable = value as IFormattable;
                if (formattable == null)
                {
                    if (value == null)
                    {
                        result = String.Empty;
                    }
                    else
                    {
                        result = value.ToString();
                    }
                }
                else
                {
                    result = formattable.ToString(format, formatProvider);
                }
            }

            return result;
        }

        private void GetConsoleColors(out ConsoleColor fg, out ConsoleColor bg)
        {
            fg = _console.ForegroundColor;
            bg = _console.BackgroundColor;
        }

        private void SetConsoleColors(ConsoleColor fg, ConsoleColor bg)
        {
            _console.ForegroundColor = fg;
            _console.BackgroundColor = bg;
        }

        private ConsoleColor GetColor(Color color, ConsoleColor @default)
        {
            if (color == null)
            {
                return @default;
            }
            return ConvertColor(color);
        }

        private void SetColors(Color foreground, Color background)
        {
            if (foreground != null)
            {
                _console.ForegroundColor = ConvertColor(foreground);
            }
            if (background != null)
            {
                _console.BackgroundColor = ConvertColor(background);
            }
        }

        private ConsoleColor ConvertColor(Color color)
        {
            if (_colorConverter.TryConvert(color, out var consoleColor))
            {
                return consoleColor;
            }
            throw new NotSupportedException($"Color {color} is not supported.");
        }

        public void Clear()
        {
            _lock.ThrowIfLocked();
            _console.Clear();
        }
    }
}
