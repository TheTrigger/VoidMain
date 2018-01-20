using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VoidMain.Application.Commands.Model
{
    public class CommandsCollection : ICommandsMutableCollection
    {
        private readonly List<CommandModel> _commands;

        public CommandsCollection()
        {
            _commands = new List<CommandModel>();
        }

        public CommandsCollection(IEnumerable<CommandModel> commands)
        {
            _commands = commands.ToList();
        }

        public int Count => _commands.Count;
        public CommandModel this[int index] => _commands[index];

        public void Add(CommandModel command)
        {
            _commands.Add(command);
        }

        public void AddRange(IEnumerable<CommandModel> commands)
        {
            _commands.AddRange(commands);
        }

        public void Remove(CommandModel command)
        {
            _commands.Remove(command);
        }

        public IEnumerator<CommandModel> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
