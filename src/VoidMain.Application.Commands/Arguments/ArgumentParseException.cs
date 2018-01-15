using System;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Arguments
{
    public class ArgumentParseException : Exception
    {
        public ArgumentModel Argument { get; }

        public ArgumentParseException(ArgumentModel argument, string message)
            : base(FormatMessage(argument, message))
        {
            Argument = argument;
        }

        public ArgumentParseException(ArgumentModel argument, string message, Exception innerException)
            : base(FormatMessage(argument, message), innerException)
        {
            Argument = argument;
        }

        private static string FormatMessage(ArgumentModel argument, string message)
        {
            return $"Could not parse argument '{argument.Name}': {message}";
        }
    }
}
