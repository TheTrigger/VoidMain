﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoidMain.Application.Commands.Arguments;
using VoidMain.Application.Commands.Execution;
using VoidMain.Application.Commands.Model;
using VoidMain.Application.Commands.Resolving;
using VoidMain.Hosting;

namespace VoidMain.Application.Commands
{
    public class CommandsApplication : ICommandsApplication
    {
        private readonly Dictionary<string, string[]> EmptyOptions = new Dictionary<string, string[]>();
        private readonly string[] EmptyOperands = new string[0];

        private readonly IServiceProvider _services;
        private readonly ICommandResolver _commandResolver;
        private readonly IArgumentsParser _argumentsParser;
        private readonly ICommandExecutor _commandExecutor;
        private readonly ApplicationModel _appModel;

        public CommandsApplication(IServiceProvider services, ICommandResolver commandResolver,
            IArgumentsParser argumentsParser, ICommandExecutor commandExecutor,
            ApplicationModel appModel)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _commandResolver = commandResolver ?? throw new ArgumentNullException(nameof(commandResolver));
            _argumentsParser = argumentsParser ?? throw new ArgumentNullException(nameof(argumentsParser));
            _commandExecutor = commandExecutor ?? throw new ArgumentNullException(nameof(commandExecutor));
            _appModel = appModel ?? throw new ArgumentNullException(nameof(appModel));
        }

        public async Task ExecuteCommand(Dictionary<string, object> context)
        {
            var token = GetCancellationToken(context);
            var options = GetOptions(context);
            var operands = GetOperands(context);
            token.ThrowIfCancellationRequested();

            using (var scope = _services.CreateScope())
            {
                var command = _commandResolver.Resolve(context, _appModel.Commands);
                var arguments = _argumentsParser.Parse(command.Arguments, options, operands);

                token.ThrowIfCancellationRequested();

                await _commandExecutor.ExecuteAsync(command, arguments, scope.ServiceProvider, token);
            }
        }

        private CancellationToken GetCancellationToken(Dictionary<string, object> context)
        {
            if (context.TryGetValue(ContextKey.CommandCanceled, out var value))
            {
                return (CancellationToken)value;
            }
            return CancellationToken.None;
        }

        private Dictionary<string, string[]> GetOptions(Dictionary<string, object> context)
        {
            if (context.TryGetValue(ContextKey.CommandOptions, out var value))
            {
                return (Dictionary<string, string[]>)value;
            }
            if (EmptyOptions.Count > 0)
            {
                EmptyOptions.Clear();
            }
            return EmptyOptions;
        }

        private string[] GetOperands(Dictionary<string, object> context)
        {
            if (context.TryGetValue(ContextKey.CommandOperands, out var value))
            {
                return (string[])value;
            }
            return EmptyOperands;
        }
    }
}
