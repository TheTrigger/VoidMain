using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public class CommandsApplicationBuilder : ICommandsApplicationBuilder
    {
        private readonly IServiceProvider _services;
        private readonly IModuleModelConstructor _moduleConstructor;
        private readonly ICommandModelConstructor _commandsConstructor;
        private readonly IModuleConfigurationFactory _configFactory;
        private readonly Dictionary<TypeInfo, ModuleModel> _modules;

        public CommandsApplicationBuilder(IServiceProvider services,
            IModuleModelConstructor moduleConstructor, ICommandModelConstructor commandsConstructor,
            IModuleConfigurationFactory configFactory)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _moduleConstructor = moduleConstructor ?? throw new ArgumentNullException(nameof(moduleConstructor));
            _commandsConstructor = commandsConstructor ?? throw new ArgumentNullException(nameof(commandsConstructor));
            _configFactory = configFactory ?? throw new ArgumentNullException(nameof(configFactory));
            _modules = new Dictionary<TypeInfo, ModuleModel>();
        }

        public void AddModule<TModule>(Action<IModuleConfiguration<TModule>> configure = null)
        {
            var moduleType = typeof(TModule).GetTypeInfo();
            if (_modules.TryGetValue(moduleType, out ModuleModel module))
            {
                throw new ArgumentException(
                    $"Module {module.Type.FullName} is already configured.",
                    nameof(TModule));
            }

            module = _moduleConstructor.Create(moduleType);

            if (configure == null)
            {
                AddCommandsToModule(moduleType, module);
            }
            else
            {
                AddCommandsAndConfigureModule(moduleType, module, configure);
            }

            _modules.Add(moduleType, module);
        }

        private void AddCommandsToModule(TypeInfo moduleType, ModuleModel module)
        {
            var methods = moduleType.GetMethods();
            foreach (var method in methods)
            {
                if (_commandsConstructor.IsCommand(method, module))
                {
                    var command = _commandsConstructor.Create(method, module);
                    module.Commands.Add(command);
                }
            }
        }

        private void AddCommandsAndConfigureModule<TModule>(
            TypeInfo moduleType, ModuleModel module,
            Action<IModuleConfiguration<TModule>> configure)
        {
            var config = _configFactory.Create<TModule>();
            configure(config);

            if (config.Name != null)
            {
                module.Name = config.Name;
            }
            if (config.Description != null)
            {
                module.Description = config.Description;
            }

            var methods = moduleType.GetMethods();
            foreach (var method in methods)
            {
                if (_commandsConstructor.IsCommand(method, module)
                    && !config.IsRemoved(method))
                {
                    var command = _commandsConstructor.Create(method, module);
                    module.Commands.Add(command);
                }
            }
        }

        public ICommandsApplication Build()
        {
            var appModel = new ApplicationModel
            {
                Modules = _modules.Values.ToList()
            };
            return new CommandsApplication(_services, appModel);
        }
    }
}
