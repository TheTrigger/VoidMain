﻿using System;
using System.Collections.Generic;
using VoidMain.Application.Commands.Arguments;
using VoidMain.Application.Commands.Model;
using VoidMain.Application.Commands.Resolving;
using VoidMain.Hosting;

namespace VoidMain.Application.Commands.Help
{
    [Module(Name = "")]
    public class HelpModule : CommandsModule
    {
        private readonly ApplicationModel _appModel;
        private readonly ICommandResolver _commandResolver;
        private readonly ICollectionConstructorProvider _colCtorProvider;
        private readonly HelpProvider _helpProvider;

        public HelpModule(ApplicationModel appModel,
            ICommandResolver commandResolver, ICollectionConstructorProvider colCtorProvider)
        {
            _appModel = appModel ?? throw new ArgumentNullException(nameof(appModel));
            _commandResolver = commandResolver ?? throw new ArgumentNullException(nameof(commandResolver));
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
            _helpProvider = new HelpProvider(colCtorProvider);
        }

        public void Help([Operand] string commandName = null)
        {
            string help = String.IsNullOrWhiteSpace(commandName)
                ? _helpProvider.GetGeneralHelp(_appModel.Commands)
                : _helpProvider.GetCommandHelp(FindCommand(commandName));

            Output.WriteLine(help);
        }

        private CommandModel FindCommand(string commandName)
        {
            var name = CommandName.Parse(commandName);
            var context = new Dictionary<string, object>
            {
                [ContextKey.CommandName] = name.Parts
            };

            return _commandResolver.Resolve(context, _appModel.Commands);
        }
    }
}