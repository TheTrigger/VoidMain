using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryFileStorage : ICommandsHistoryStorage
    {
        private readonly string _filePath;
        private readonly Encoding _encoding;

        public CommandsHistoryFileStorage(
            CommandsHistoryFileStorageOptions options = null)
        {
            _filePath = options?.FilePath;
            if (_filePath == null)
            {
                _filePath = Assembly.GetEntryAssembly().Location + ".history";
            }
            else if (!Path.IsPathRooted(_filePath))
            {
                _filePath = Path.GetFullPath(_filePath);
            }
            _encoding = options?.Encoding ?? Encoding.UTF8;
        }

        public string[] Load()
        {
            if (!File.Exists(_filePath))
            {
                return Array.Empty<string>();
            }

            var commands = File.ReadLines(_filePath, _encoding)
                .Where(_ => !String.IsNullOrWhiteSpace(_));

            return commands.ToArray();
        }

        public void Save(string[] commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            File.WriteAllLines(_filePath, commands, _encoding);
        }
    }
}
