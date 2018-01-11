using System.Reflection;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public interface ICommandModelConstructor
    {
        bool IsCommand(MethodInfo method, ModuleModel module);
        CommandModel Create(MethodInfo method, ModuleModel module);
    }
}
