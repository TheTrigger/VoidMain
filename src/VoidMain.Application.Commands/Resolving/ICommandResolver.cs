using System.Collections.Generic;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Resolving
{
    public interface ICommandResolver
    {
        CommandModel Resolve(Dictionary<string, object> context, IReadOnlyList<CommandModel> commands);
        bool TryResolve(Dictionary<string, object> context, IReadOnlyList<CommandModel> commands, out CommandModel command);
    }
}
