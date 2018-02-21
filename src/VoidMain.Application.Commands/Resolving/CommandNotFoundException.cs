using System;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Resolving
{
    public class CommandNotFoundException : Exception
    {
        public CommandName CommandName { get; }

        public CommandNotFoundException(CommandName commandName)
            : this(commandName, null, null)
        {
        }

        public CommandNotFoundException(CommandName commandName, string message)
            : this(commandName, message, null)
        {
        }

        public CommandNotFoundException(CommandName commandName, string message, Exception inner)
            : base(FormatMessage(commandName, message), inner)
        {
            CommandName = commandName;
        }

        private static string FormatMessage(CommandName commandName, string message = null)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                return $"Command '{commandName}' was not found.";
            }
            return $"Command '{commandName}' was not found: {message}";
        }
    }
}
