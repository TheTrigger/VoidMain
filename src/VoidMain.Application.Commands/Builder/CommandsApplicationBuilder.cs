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
        private readonly Dictionary<TypeInfo, ModuleModel> _modules;

        public CommandsApplicationBuilder(IServiceProvider services,
            IModuleModelConstructor moduleConstructor, ICommandModelConstructor commandsConstructor)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _moduleConstructor = moduleConstructor ?? throw new ArgumentNullException(nameof(moduleConstructor));
            _commandsConstructor = commandsConstructor ?? throw new ArgumentNullException(nameof(commandsConstructor));
            _modules = new Dictionary<TypeInfo, ModuleModel>();
        }

        public void AddModule<TModule>()
        {
            var moduleType = typeof(TModule).GetTypeInfo();
            if (_modules.TryGetValue(moduleType, out ModuleModel module))
            {
                throw new ArgumentException(
                    $"Module {module.Type.FullName} is already configured.",
                    nameof(TModule));
            }

            module = _moduleConstructor.Create(moduleType);

            var methods = moduleType.GetMethods();

            foreach (var method in methods)
            {
                if (_commandsConstructor.IsCommand(method, module))
                {
                    var command = _commandsConstructor.Create(method, module);
                    module.Commands.Add(command);
                }
            }

            _modules.Add(moduleType, module);
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
