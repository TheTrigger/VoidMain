using System;
using System.Collections.Generic;
using System.Linq;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Builder;
using VoidMain.Application.Commands.Model;
using VoidMain.Application.Commands.Resolving;

namespace VoidMain.Application.Commands.Help
{
    [Module(ExcludeFromCommandName = true)]
    public class HelpModule : CommandsModule
    {
        private readonly ApplicationModel _appModel;
        private readonly ICommandResolver _commandResolver;
        private readonly ICommandNameParser _commandNameParser;
        private readonly HelpWriter _helpWriter;

        public HelpModule(ApplicationModel appModel,
            ICommandResolver commandResolver,
            ICommandNameParser commandNameParser,
            ICollectionConstructorProvider colCtorProvider)
        {
            _appModel = appModel ?? throw new ArgumentNullException(nameof(appModel));
            _commandResolver = commandResolver ?? throw new ArgumentNullException(nameof(commandResolver));
            _commandNameParser = commandNameParser ?? throw new ArgumentNullException(nameof(commandNameParser));
            if (colCtorProvider == null)
            {
                throw new ArgumentNullException(nameof(colCtorProvider));
            }
            _helpWriter = new HelpWriter(colCtorProvider);
        }

        [Command(Description = "Display help information")]
        public void Help([Operand(Description = "Name of the command")] string[] commandName = null)
        {
            if (commandName == null || commandName.Length == 0)
            {
                _helpWriter.WriteGeneralHelp(Output, _appModel.Commands);
            }
            else
            {
                var command = FindCommand(commandName);
                _helpWriter.WriteCommandHelp(Output, command);
            }
        }

        private CommandModel FindCommand(string[] commandName)
        {
            var name = _commandNameParser.Parse(String.Join(" ", commandName));
            var context = new Dictionary<string, object>();
            ContextHelper.SetCommandName(context, name.Parts as string[] ?? name.Parts.ToArray());

            return _commandResolver.Resolve(context, _appModel.Commands);
        }
    }
}
