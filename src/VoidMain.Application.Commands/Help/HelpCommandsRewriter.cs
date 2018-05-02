using System;
using System.Collections.Generic;
using System.Linq;
using VoidMain.Application.Commands.Model;
using VoidMain.Application.Commands.Resolving;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Help
{
    public class HelpCommandsRewriter
    {
        private readonly string HelpName = "help";

        private readonly ApplicationModel _appModel;
        private readonly ICommandResolver _commandResolver;
        private readonly CommandLineOptions _cliOptions;

        public HelpCommandsRewriter(
            ApplicationModel appModel,
            ICommandResolver commandResolver,
            CommandLineOptions cliOptions = null)
        {
            _appModel = appModel ?? throw new ArgumentNullException(nameof(appModel));
            _commandResolver = commandResolver ?? throw new ArgumentNullException(nameof(commandResolver));
            _cliOptions = cliOptions ?? new CommandLineOptions();
            _cliOptions.Validate();
        }

        public bool TryRewrite(Dictionary<string, object> context)
        {
            if (!ContextHelper.TryGetOptions(context, out var options))
            {
                return false;
            }

            if (!options.Any(IsHelpOption))
            {
                return false;
            }

            if (ContextHelper.TryGetCommandName(context, out var commandNameParts) && commandNameParts.Length > 0)
            {
                if (_commandResolver.TryResolve(context, _appModel.Commands, out var command)
                    && command.Arguments.Any(IsHelpOption))
                {
                    return false;
                }

                var commandName = new CommandName(commandNameParts);
                ContextHelper.SetOperands(context, new[] { commandName.ToString() });
            }
            else
            {
                // General help
                ContextHelper.SetOperands(context, ContextHelper.EmptyOperands);
            }

            ContextHelper.SetCommandName(context, new[] { "help" });
            ContextHelper.SetOptions(context, ContextHelper.EmptyOptions);

            return true;
        }

        private bool IsHelpOption(KeyValuePair<string, string> option)
        {
            return _cliOptions.IdentifierComparer.Equals(option.Key, HelpName);
        }

        private bool IsHelpOption(ArgumentModel arg)
        {
            return arg.Kind == ArgumentKind.Option
                && _cliOptions.IdentifierComparer.Equals(arg.Name, HelpName);
        }
    }
}
