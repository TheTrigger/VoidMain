using System;
using VoidMain.CommandLineIinterface.IO.Console;

namespace VoidMain.CommandLineIinterface.IO.Prompt
{
    public class ConsolePromptMessage : IConsolePromptMessage
    {
        private readonly IConsole _console;
        private readonly ConsolePromptMessageOptions _options;

        public ConsolePromptMessage(IConsole console, ConsolePromptMessageOptions options = null)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _options = options ?? new ConsolePromptMessageOptions();
            _options.Validate();
        }

        public void Print()
        {
            _console.Write(_options.Message);
        }
    }
}
