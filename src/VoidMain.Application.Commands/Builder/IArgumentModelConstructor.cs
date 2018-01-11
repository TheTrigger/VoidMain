using System.Reflection;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Builder
{
    public interface IArgumentModelConstructor
    {
        ArgumentModel Create(ParameterInfo parameter, CommandModel command);
    }
}
