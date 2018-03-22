using System;
using System.Collections.Generic;
using System.Linq;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface;
using VoidMain.Hosting;

namespace VoidMain.Application.Commands.Resolving
{
    public class CommandResolver : ICommandResolver
    {
        private readonly IEqualityComparer<CommandName> _nameComparer;

        public CommandResolver(CommandLineSyntaxOptions syntaxOptions = null)
        {
            var identifierComparer = syntaxOptions?.IdentifierComparer
                ?? CommandLineSyntaxOptions.DefaultIdentifierComparer;
            _nameComparer = new CommandNameComparer(identifierComparer);
        }

        public CommandModel Resolve(Dictionary<string, object> context, IReadOnlyList<CommandModel> commands)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            if (!ContextHelper.TryGetCommandName(context, out var commandNameParts) || commandNameParts.Length == 0)
            {
                throw new InvalidCommandNameException("Command name is not specified.");
            }

            var commandName = new CommandName(commandNameParts);

            var command = commands.FirstOrDefault(_ => _nameComparer.Equals(_.Name, commandName));
            if (command == null)
            {
                throw new CommandNotFoundException(commandName);
            }

            return command;
        }

        public bool TryResolve(Dictionary<string, object> context, IReadOnlyList<CommandModel> commands, out CommandModel command)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            if (!ContextHelper.TryGetCommandName(context, out var commandNameParts) || commandNameParts.Length == 0)
            {
                command = null;
                return false;
            }

            var commandName = new CommandName(commandNameParts);

            command = commands.FirstOrDefault(_ => _nameComparer.Equals(_.Name, commandName));
            return command != null;
        }
    }
}
