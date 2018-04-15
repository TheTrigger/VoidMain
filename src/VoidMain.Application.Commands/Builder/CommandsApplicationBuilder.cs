using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VoidMain.Application.Commands.Builder.Validation;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public class CommandsApplicationBuilder : ICommandsApplicationBuilder
    {
        private readonly IModuleModelConstructor _moduleConstructor;
        private readonly ICommandModelConstructor _commandsConstructor;
        private readonly IModuleConfigurationFactory _configFactory;
        private readonly ICommandModelValidator _commandValidator;
        private readonly List<CommandModel> _commands;

        public IServiceProvider Services { get; }

        public CommandsApplicationBuilder(IServiceProvider services,
            IModuleModelConstructor moduleConstructor, ICommandModelConstructor commandsConstructor,
            IModuleConfigurationFactory configFactory, ICommandModelValidator commandValidator)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            _moduleConstructor = moduleConstructor ?? throw new ArgumentNullException(nameof(moduleConstructor));
            _commandsConstructor = commandsConstructor ?? throw new ArgumentNullException(nameof(commandsConstructor));
            _configFactory = configFactory ?? throw new ArgumentNullException(nameof(configFactory));
            _commandValidator = commandValidator ?? throw new ArgumentNullException(nameof(commandValidator));
            _commands = new List<CommandModel>();
        }

        public void AddModule<TModule>(Action<IModuleConfiguration<TModule>> configure = null)
        {
            var moduleType = typeof(TModule).GetTypeInfo();
            var module = _moduleConstructor.Create(moduleType);

            var config = _configFactory.Create<TModule>();
            config.Name = module.Name;
            config.Description = module.Description;
            config.ExcludeFromCommandName = module.ExcludeFromCommandName;
            configure?.Invoke(config);

            AddModuleInternal(module, config);
        }

        private void AddModuleInternal<TModule>(ModuleModel module, IModuleConfiguration<TModule> config)
        {
            module.Name = config.Name;
            module.Description = config.Description;
            module.ExcludeFromCommandName = config.ExcludeFromCommandName;

            var newCommands = new List<CommandModel>();
            var methods = module.Type.GetTypeInfo().GetMethods();

            foreach (var method in methods)
            {
                if (config.IsRemoved(method))
                {
                    continue;
                }

                if (_commandsConstructor.TryCreate(method, module, out var command))
                {
                    var result = _commandValidator.Validate(command);
                    if (!result.IsValid)
                    {
                        throw new InvalidCommandException(command, result.Errors);
                    }

                    module.Commands.Add(command);
                    newCommands.Add(command);
                }
            }

            // TODO: test for command duplicates

            _commands.AddRange(newCommands);
        }

        public ICommandsApplication Build()
        {
            var appModel = Services.GetRequiredService<ApplicationModel>();
            appModel.Commands = _commands.ToList();
            var app = ActivatorUtilities.CreateInstance<CommandsApplication>(Services);
            return app;
        }
    }
}
