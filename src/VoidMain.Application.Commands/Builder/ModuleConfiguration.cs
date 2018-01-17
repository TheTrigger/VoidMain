using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace VoidMain.Application.Commands.Builder
{
    public class ModuleConfiguration<TModule> : IModuleConfiguration<TModule>
    {
        private List<MethodInfo> _removed;

        public string Name { get; set; }
        public string Description { get; set; }

        public ModuleConfiguration()
        {
            _removed = new List<MethodInfo>();
        }

        public void RemoveCommand(Expression<Action<TModule>> command)
        {
            var body = command.Body;
            if (body is MethodCallExpression call)
            {
                var method = call.Method;
                if (method.DeclaringType != typeof(TModule))
                {
                    throw new CommandsConfigurationException(
                        $"Removed command must be declared in the '{typeof(TModule).Name}' type.");
                }
                _removed.Add(method);
            }
            else
            {
                throw new CommandsConfigurationException("Expression must be a method call.");
            }
        }

        public bool IsRemoved(MethodInfo method)
        {
            return _removed.Contains(method);
        }
    }
}
