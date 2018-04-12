using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryFileStorage : ICommandsHistoryStorage
    {
        private readonly CommandsHistoryFileStorageOptions _options;

        public CommandsHistoryFileStorage(
            CommandsHistoryFileStorageOptions options = null)
        {
            _options = options ?? new CommandsHistoryFileStorageOptions();
            _options.Validate();

            if (!Path.IsPathRooted(_options.FilePath))
            {
                _options.FilePath = Path.GetFullPath(_options.FilePath);
            }
        }

        public IReadOnlyList<string> Load()
        {
            if (!File.Exists(_options.FilePath))
            {
                return Array.Empty<string>();
            }

            var commands = File.ReadLines(_options.FilePath, _options.Encoding)
                .Where(_ => !String.IsNullOrWhiteSpace(_));

            return commands.ToArray();
        }

        public void Save(IReadOnlyList<string> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            File.WriteAllLines(_options.FilePath, commands, _options.Encoding);
        }
    }
}
