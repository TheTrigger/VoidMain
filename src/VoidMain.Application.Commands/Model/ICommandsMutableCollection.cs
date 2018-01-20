using System.Collections.Generic;

namespace VoidMain.Application.Commands.Model
{
    public interface ICommandsMutableCollection : ICommandsCollection
    {
        void Add(CommandModel command);
        void AddRange(IEnumerable<CommandModel> commands);
        void Remove(CommandModel command);
    }
}
