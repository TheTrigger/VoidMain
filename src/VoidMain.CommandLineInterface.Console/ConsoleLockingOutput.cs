using System;
using System.Collections.Generic;
using VoidMain.CommandLineInterface.IO;
using VoidMain.CommandLineInterface.IO.Console;
using VoidMain.CommandLineInterface.IO.Templates;

namespace VoidMain.CommandLineInterface
{
    public class ConsoleLockingOutput : ICommandLineOutput
    {
        private readonly Type CustomFormatterType = typeof(ICustomFormatter);

        private readonly IConsole _console;
        private readonly IColoredTextWriter _coloredTextWriter;
        private readonly IMessageTemplateParser _templateParser;
        private readonly IMessageTemplateWriter _templateWriter;
        private readonly IMessageTemplateColoredWriter _templateColoredWriter;
        private readonly ConsoleOutputLock _lock;

        public ConsoleLockingOutput(
            IConsole console,
            IColoredTextWriter coloredTextWriter,
            IMessageTemplateParser templateParser,
            IMessageTemplateWriter templateWriter,
            IMessageTemplateColoredWriter templateColoredWriter,
            ConsoleOutputLock @lock)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _coloredTextWriter = coloredTextWriter ?? throw new ArgumentNullException(nameof(coloredTextWriter));
            _templateParser = templateParser ?? throw new ArgumentNullException(nameof(templateParser));
            _templateWriter = templateWriter ?? throw new ArgumentNullException(nameof(templateWriter));
            _templateColoredWriter = templateColoredWriter ?? throw new ArgumentNullException(nameof(templateColoredWriter));
            _lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
        }

        public void Write(char value)
        {
            _lock.ThrowIfLocked();
            _console.Write(value);
        }

        public void Write(char value, int count)
        {
            _lock.ThrowIfLocked();
            _console.Write(value, count);
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
            _templateWriter.Write(parsedTemplate, args, _coloredTextWriter, formatProvider);
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
            _coloredTextWriter.Write(foreground, null, value);
        }

        public void Write(Color foreground, char value, int count)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, null, value, count);
        }

        public void Write(Color foreground, string value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, null, value);
        }

        public void Write(Color foreground, object value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, null, value);
        }

        public void WriteLine(Color foreground, string value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, null, value);
            _coloredTextWriter.WriteLine();
        }

        public void WriteLine(Color foreground, object value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, null, value);
            _coloredTextWriter.WriteLine();
        }

        public void Write(Color foreground, Color background, char value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, background, value);
        }

        public void Write(Color foreground, Color background, char value, int count)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, background, value, count);
        }

        public void Write(Color foreground, Color background, string value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, background, value);
        }

        public void Write(Color foreground, Color background, object value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, background, value);
        }

        public void WriteLine(Color foreground, Color background, string value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, background, value);
            _coloredTextWriter.WriteLine();
        }

        public void WriteLine(Color foreground, Color background, object value)
        {
            _lock.ThrowIfLocked();
            _coloredTextWriter.Write(foreground, background, value);
            _coloredTextWriter.WriteLine();
        }

        public void Write(ColoredFormat format)
        {
            Write(null, format);
        }

        public void Write(IFormatProvider formatProvider, ColoredFormat format)
        {
            _lock.ThrowIfLocked();

            var template = format.Template;
            var parsedTemplate = _templateParser.Parse(template.Value);
            var coloredTemplate = new Colored<MessageTemplate>(
                parsedTemplate, template.Foreground, template.Background);
            IReadOnlyList<Colored<object>> coloredArgs = format;
            _templateColoredWriter.Write(coloredTemplate, coloredArgs, _coloredTextWriter, formatProvider);
        }

        public void WriteLine(ColoredFormat format)
        {
            Write(null, format);
            _console.WriteLine();
        }

        public void WriteLine(IFormatProvider formatProvider, ColoredFormat format)
        {
            Write(formatProvider, format);
            _console.WriteLine();
        }

        public void Clear()
        {
            _lock.ThrowIfLocked();
            _console.Clear();
        }
    }
}
