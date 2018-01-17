using System;

namespace VoidMain.Application.Commands.Builder
{
    public class CommandsConfigurationException : Exception
    {
        public CommandsConfigurationException() { }
        public CommandsConfigurationException(string message)
            : base(message) { }
        public CommandsConfigurationException(string message, Exception inner)
            : base(message, inner) { }
    }
}
