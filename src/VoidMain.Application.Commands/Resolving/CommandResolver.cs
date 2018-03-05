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

            if (!context.TryGetValue(ContextKey.CommandName, out var value))
            {
                throw new InvalidCommandNameException("Command name is not specified.");
            }

            var nameParts = (string[])value;
            if (nameParts.Length == 0)
            {
                throw new InvalidCommandNameException("Command name is empty.");
            }

            var name = new CommandName(nameParts);

            var command = commands.FirstOrDefault(_ => _nameComparer.Equals(_.Name, name));
            if (command == null)
            {
                throw new CommandNotFoundException(name);
            }

            return command;
        }
    }
}
