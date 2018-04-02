using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.History
{
    public interface ICommandsHistoryStorage
    {
        IReadOnlyList<string> Load();
        void Save(IReadOnlyList<string> commands);
    }
}
