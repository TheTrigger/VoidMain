using System;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public class CommandNameParser : ICommandNameParser
    {
        private readonly char[] NameSeparators = new[] { ' ' };

        public CommandName Parse(string commandName)
        {
            if (commandName == null)
            {
                throw new ArgumentNullException(nameof(commandName));
            }
            var nameParts = commandName.Split(NameSeparators, StringSplitOptions.RemoveEmptyEntries);
            return new CommandName(nameParts);
        }
    }
}
