using System;
using System.Linq;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryInMemoryStorage : ICommandsHistoryStorage
    {
        private string[] _commands;

        public CommandsHistoryInMemoryStorage(
            CommandsHistoryInMemoryStorageOptions options = null)
        {
            _commands = options?.Commands ?? Array.Empty<string>();
        }

        public string[] Load()
        {
            return _commands.ToArray();
        }

        public void Save(string[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            _commands = commands.ToArray();
        }
    }
}
