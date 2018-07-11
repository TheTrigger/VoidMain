using System;
using System.Reflection;
using System.Text;

namespace VoidMain.CommandLineIinterface.History
{
    public class CommandsHistoryFileStorageOptions
    {
        public string FilePath { get; set; }
        public Encoding Encoding { get; set; }

        public CommandsHistoryFileStorageOptions(bool defaults = true)
        {
            if (defaults)
            {
                FilePath = Assembly.GetEntryAssembly().Location + ".history";
                Encoding = Encoding.UTF8;
            }
        }

        public void Validate()
        {
            if (String.IsNullOrWhiteSpace(FilePath))
            {
                throw new ArgumentNullException(nameof(FilePath));
            }
            if (Encoding == null)
            {
                throw new ArgumentNullException(nameof(Encoding));
            }
        }
    }
}
