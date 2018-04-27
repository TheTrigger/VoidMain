using System;
using VoidMain.CommandLineIinterface.IO;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface
{
    public class ConsoleLockingOutput : ICommandLineOutput
    {
        private readonly IConsole _console;
        private readonly IConsoleColorConverter _colorConverter;
        private readonly ConsoleOutputLock _lock;

        public ConsoleLockingOutput(IConsole console, IConsoleColorConverter colorConverter, ConsoleOutputLock @lock)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _colorConverter = colorConverter ?? throw new ArgumentNullException(nameof(colorConverter));
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
            _lock.ThrowIfLocked();
            _console.Write(format, args);
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
            _lock.ThrowIfLocked();
            _console.WriteLine(format, args);
        }

        public void Write(Color foreground, char value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.Write(value);
            _console.ForegroundColor = pf;
        }

        public void Write(Color foreground, string value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.Write(value);
            _console.ForegroundColor = pf;
        }

        public void Write(Color foreground, object value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.Write(value);
            _console.ForegroundColor = pf;
        }

        public void Write(Color foreground, string format, params object[] args)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.Write(format, args);
            _console.ForegroundColor = pf;
        }

        public void WriteLine(Color foreground, string value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.WriteLine(value);
            _console.ForegroundColor = pf;
        }

        public void WriteLine(Color foreground, object value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.WriteLine(value);
            _console.ForegroundColor = pf;
        }

        public void WriteLine(Color foreground, string format, params object[] args)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.WriteLine(format, args);
            _console.ForegroundColor = pf;
        }

        public void Write(Color foreground, Color background, char value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            var pb = _console.BackgroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.BackgroundColor = ConvertColor(background);
            _console.Write(value);
            _console.ForegroundColor = pf;
            _console.BackgroundColor = pb;
        }

        public void Write(Color foreground, Color background, string value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            var pb = _console.BackgroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.BackgroundColor = ConvertColor(background);
            _console.Write(value);
            _console.ForegroundColor = pf;
            _console.BackgroundColor = pb;
        }

        public void Write(Color foreground, Color background, object value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            var pb = _console.BackgroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.BackgroundColor = ConvertColor(background);
            _console.Write(value);
            _console.ForegroundColor = pf;
            _console.BackgroundColor = pb;
        }

        public void Write(Color foreground, Color background, string format, params object[] args)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            var pb = _console.BackgroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.BackgroundColor = ConvertColor(background);
            _console.Write(format, args);
            _console.ForegroundColor = pf;
            _console.BackgroundColor = pb;
        }

        public void WriteLine(Color foreground, Color background, string value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            var pb = _console.BackgroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.BackgroundColor = ConvertColor(background);
            _console.WriteLine(value);
            _console.ForegroundColor = pf;
            _console.BackgroundColor = pb;
        }

        public void WriteLine(Color foreground, Color background, object value)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            var pb = _console.BackgroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.BackgroundColor = ConvertColor(background);
            _console.WriteLine(value);
            _console.ForegroundColor = pf;
            _console.BackgroundColor = pb;
        }

        public void WriteLine(Color foreground, Color background, string format, params object[] args)
        {
            _lock.ThrowIfLocked();
            var pf = _console.ForegroundColor;
            var pb = _console.BackgroundColor;
            _console.ForegroundColor = ConvertColor(foreground);
            _console.BackgroundColor = ConvertColor(background);
            _console.WriteLine(format, args);
            _console.ForegroundColor = pf;
            _console.BackgroundColor = pb;
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
