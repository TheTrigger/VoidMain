using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VoidMain.Hosting.FileSystem;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryFileStorage : ICommandsHistoryStorage
    {
        private readonly IFileSystem _fileSystem;
        private readonly CommandsHistoryFileStorageOptions _options;

        public CommandsHistoryFileStorage(IFileSystem fileSystem,
            CommandsHistoryFileStorageOptions options = null)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _options = options ?? new CommandsHistoryFileStorageOptions();
            _options.Validate();

            if (!Path.IsPathRooted(_options.FilePath))
            {
                _options.FilePath = Path.GetFullPath(_options.FilePath);
            }
        }

        public IReadOnlyList<string> Load()
        {
            if (!_fileSystem.FileExists(_options.FilePath))
            {
                return Array.Empty<string>();
            }

            var commands = _fileSystem.ReadAllLines(_options.FilePath, _options.Encoding)
                .Where(_ => !String.IsNullOrWhiteSpace(_));

            return commands.ToArray();
        }

        public void Save(IReadOnlyList<string> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            _fileSystem.WriteAllLines(_options.FilePath, commands, _options.Encoding);
        }
    }
}
