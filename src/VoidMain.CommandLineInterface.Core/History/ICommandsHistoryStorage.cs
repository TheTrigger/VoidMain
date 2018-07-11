using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.History
{
    public interface ICommandsHistoryStorage
    {
        IReadOnlyList<string> Load();
        void Save(IReadOnlyList<string> commands);
    }
}
