using System;
using System.Reflection;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public class CommandModelConstructor : ICommandModelConstructor
    {
        private readonly ICommandNameParser _commandNameParser;
        private readonly IArgumentModelConstructor _argumentConstructor;

        public CommandModelConstructor(ICommandNameParser commandNameParser, IArgumentModelConstructor argumentConstructor)
        {
            _commandNameParser = commandNameParser ?? throw new ArgumentNullException(nameof(commandNameParser));
            _argumentConstructor = argumentConstructor ?? throw new ArgumentNullException(nameof(argumentConstructor));
        }

        public bool IsCommand(MethodInfo method, ModuleModel module)
        {
            return GetError(method, module) == null;
        }

        public bool TryCreate(MethodInfo method, ModuleModel module, out CommandModel command)
        {
            if (!IsCommand(method, module))
            {
                command = null;
                return false;
            }

            command = CreateInternal(method, module);
            return true;
        }

        public CommandModel Create(MethodInfo method, ModuleModel module)
        {
            Validate(method, module);
            return CreateInternal(method, module);
        }

        private CommandModel CreateInternal(MethodInfo method, ModuleModel module)
        {
            var command = new CommandModel();
            command.Method = method;
            command.Module = module;

            string name = null;
            var attr = method.GetCustomAttribute<CommandAttribute>();
            if (attr == null)
            {
                name = method.Name;
            }
            else
            {
                name = attr.Name ?? method.Name;
                command.Description = attr.Description;
            }

            if (!String.IsNullOrWhiteSpace(module.Name))
            {
                name = module.Name + " " + name;
            }
            command.Name = _commandNameParser.Parse(name);

            var parameters = method.GetParameters();
            var arguments = new ArgumentModel[parameters.Length];
            command.Arguments = arguments;

            for (int i = 0; i < parameters.Length; i++)
            {
                arguments[i] = _argumentConstructor.Create(parameters[i], command);
            }

            return command;
        }

        private void Validate(MethodInfo method, ModuleModel module)
        {
            string errorMessage = GetError(method, module);
            if (errorMessage != null)
            {
                throw new ArgumentException(
                    $"Invalid method {method} of type {module.Type}: {errorMessage}",
                    nameof(method));
            }
        }

        private string GetError(MethodInfo method, ModuleModel module)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            if (!method.IsPublic)
            {
                return "The command must be a public method.";
            }

            if (method.IsStatic)
            {
                return "The command must be a non-static method.";
            }

            if (method.IsAbstract)
            {
                return "The command must be a non-abstract method.";
            }

            if (method.IsGenericMethod)
            {
                return "The command must be a non-generic method.";
            }

            if (method.IsSpecialName)
            {
                return "The command must not be a compile generated method.";
            }

            if (method.IsObjectBaseClassMethod())
            {
                return "The command must not be one of the object's base methods.";
            }

            if (method.IsDisposeMethod())
            {
                return "The command must not be the IDisposable.Dispose method.";
            }

            if (method.IsDefined(typeof(NonCommandAttribute)))
            {
                return "The command must not have a NonCommand attribute.";
            }

            return null;
        }
    }
}
