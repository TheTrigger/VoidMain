using System.Reflection;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public interface IModuleModelConstructor
    {
        bool IsModule(TypeInfo type);
        ModuleModel Create(TypeInfo type);
        bool TryCreate(TypeInfo type, out ModuleModel module);
    }
}
