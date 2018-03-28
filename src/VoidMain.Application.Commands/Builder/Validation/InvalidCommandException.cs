using System;
using System.Collections.Generic;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder.Validation
{
    public class InvalidCommandException : Exception
    {
        public CommandModel Command { get; }
        public IReadOnlyList<string> Errors { get; }

        public InvalidCommandException(CommandModel command, IReadOnlyList<string> errors)
            : this(null, null, command, errors)
        {
        }

        public InvalidCommandException(string message, CommandModel command, IReadOnlyList<string> errors)
            : this(message, null, command, errors)
        {
        }

        public InvalidCommandException(string message, Exception inner, CommandModel command, IReadOnlyList<string> errors)
            : base(FormatMessage(command, message), inner)
        {
            Command = command;
            Errors = errors ?? Array.Empty<string>();
        }

        private static string FormatMessage(CommandModel command, string message = null)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return $"Invalid command `{command.Method.ToDisplayString()}`.";
            }
            return $"Invalid command `{command.Method.ToDisplayString()}`: {message}";
        }
    }
}
