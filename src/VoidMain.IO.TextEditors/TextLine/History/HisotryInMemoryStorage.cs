using System.Collections.Generic;

namespace VoidMain.IO.TextEditors.TextLine.History
{
    public class HisotryInMemoryStorage : IHistoryStorage
    {
        private List<string> _history;

        public HisotryInMemoryStorage(HisotryInMemoryStorageOptions? options = null)
        {
            _history = new List<string>();
            var lines = options?.Hisotry;
            if (lines == null) return;
            _history.AddRange(lines);
        }

        public IEnumerable<string> Load() => _history;

        public void Save(IEnumerable<string> history)
        {
            _history.Clear();
            _history.AddRange(history);
        }
    }
}
