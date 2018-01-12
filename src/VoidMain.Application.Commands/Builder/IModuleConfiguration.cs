using System;
using System.Linq.Expressions;
using System.Reflection;

namespace VoidMain.Application.Commands.Builder
{
    public interface IModuleConfiguration<TModule>
    {
        string Name { get; set; }
        string Description { get; set; }
        void RemoveCommand(Expression<Action<TModule>> command);
        bool IsRemoved(MethodInfo method);
    }
}
