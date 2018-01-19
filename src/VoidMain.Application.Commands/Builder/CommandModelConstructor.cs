using System;
using System.Reflection;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public class CommandModelConstructor : ICommandModelConstructor
    {
        private readonly IArgumentModelConstructor _argumentConstructor;
        private readonly TypeInfo DisposableType;
        private readonly string DisposableName;

        public CommandModelConstructor(IArgumentModelConstructor argumentConstructor)
        {
            _argumentConstructor = argumentConstructor ?? throw new ArgumentNullException(nameof(argumentConstructor));
            DisposableType = typeof(IDisposable).GetTypeInfo();
            DisposableName = nameof(IDisposable.Dispose);
        }

        public bool IsCommand(MethodInfo method, ModuleModel module)
        {
            return GetError(method, module) == null;
        }

        public CommandModel Create(MethodInfo method, ModuleModel module)
        {
            Validate(method, module);

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
            command.Name = CommandName.Parse(name);

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

            if (IsObjectBaseClassMethod(method))
            {
                return "The command must not be one of the object's base methods.";
            }

            if (IsDisposeMethod(method))
            {
                return "The command must not be the IDisposable.Dispose method.";
            }

            if (method.IsDefined(typeof(NonCommandAttribute)))
            {
                return "The command must not have a NonCommand attribute.";
            }

            return null;
        }

        private bool IsObjectBaseClassMethod(MethodInfo method)
        {
            return method.GetBaseDefinition().DeclaringType == typeof(Object);
        }

        private bool IsDisposeMethod(MethodInfo method)
        {
            return method.Name == DisposableName
                && DisposableType.IsAssignableFrom(method.DeclaringType);
        }
    }
}
