using System;
using System.Collections.Generic;
using VoidMain.IO.FileSystem;

namespace VoidMain.IO.TextEditors.TextLine.History
{
    public class HistoryFileStorage : IHistoryStorage
    {
        private readonly IFileSystem _fileSystem;
        private readonly HistoryFileStorageOptions _options;

        public HistoryFileStorage(
            IFileSystem fileSystem,
            HistoryFileStorageOptions? options = null)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _options = options ?? new HistoryFileStorageOptions();
        }

        public IEnumerable<string> Load()
        {
            if (!_fileSystem.FileExists(_options.FilePath))
            {
                return Array.Empty<string>();
            }

            var lines = _fileSystem.ReadAllLines(_options.FilePath, _options.Encoding);
            return lines;
        }

        public void Save(IEnumerable<string> history)
        {
            _fileSystem.WriteAllLines(_options.FilePath, history, _options.Encoding);
        }
    }
}
