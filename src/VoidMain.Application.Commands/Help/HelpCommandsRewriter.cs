using System;
using System.Collections.Generic;
using System.Linq;
using VoidMain.Application.Commands.Model;
using VoidMain.Application.Commands.Resolving;
using VoidMain.CommandLineIinterface.Parser;
using VoidMain.Hosting;

namespace VoidMain.Application.Commands.Help
{
    public class HelpCommandsRewriter
    {
        private readonly string HelpName = "help";

        private readonly ApplicationModel _appModel;
        private readonly ICommandResolver _commandResolver;
        private readonly IEqualityComparer<string> _identifierComparer;

        public HelpCommandsRewriter(
            ApplicationModel appModel, ICommandResolver commandResolver,
            CommandLineSyntaxOptions syntaxOptions = null)
        {
            _appModel = appModel ?? throw new ArgumentNullException(nameof(appModel));
            _commandResolver = commandResolver ?? throw new ArgumentNullException(nameof(commandResolver));
            _identifierComparer = syntaxOptions?.IdentifierComparer
                ?? CommandLineSyntaxOptions.DefaultIdentifierComparer;
        }

        public bool TryRewrite(Dictionary<string, object> context)
        {
            if (!context.TryGetValue(ContextKey.CommandOptions, out var optionsValue))
            {
                return false;
            }

            var options = (KeyValuePair<string, string>[])optionsValue;

            bool hasHelpOption = options.Any(IsHelpOption);
            if (!hasHelpOption)
            {
                return false;
            }

            if (!context.TryGetValue(ContextKey.CommandName, out var commandNameValue))
            {
                return false;
            }

            var name = new CommandName((string[])commandNameValue);
            if (name.Parts.Count == 0)
            {
                return false;
            }

            var command = _commandResolver.Resolve(context, _appModel.Commands);
            bool hasSelfHelpOption = command.Arguments.Any(IsHelpOption);
            if (hasSelfHelpOption)
            {
                return false;
            }

            context[ContextKey.CommandName] = new[] { "help" };
            context[ContextKey.CommandOperands] = new[] { name.ToString() };

            return true;
        }

        private bool IsHelpOption(KeyValuePair<string, string> option)
        {
            return _identifierComparer.Equals(option.Key, HelpName);
        }

        private bool IsHelpOption(ArgumentModel arg)
        {
            return arg.Kind == ArgumentKind.Option
                && _identifierComparer.Equals(arg.Name, HelpName);
        }
    }
}
