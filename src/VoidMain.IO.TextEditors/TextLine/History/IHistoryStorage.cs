using System.Collections.Generic;

namespace VoidMain.IO.TextEditors.TextLine.History
{
    public interface IHistoryStorage
    {
        IEnumerable<string> Load();
        void Save(IEnumerable<string> history);
    }
}
