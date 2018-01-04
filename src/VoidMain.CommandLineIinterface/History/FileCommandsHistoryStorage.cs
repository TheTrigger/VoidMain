using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VoidMain.CommandLineIinterface.History
{
    public class FileCommandsHistoryStorage : ICommandsHistoryStorage
    {
        private readonly string _filePath;
        private readonly Encoding _encoding;

        public FileCommandsHistoryStorage()
        {
            // TODO: Comfigure file path.
            _filePath = Assembly.GetEntryAssembly().Location + ".history";
            // TODO: Configure encoding.
            _encoding = Encoding.UTF8;
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
