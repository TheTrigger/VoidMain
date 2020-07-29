using System.Text;

namespace VoidMain.IO.TextEditors.TextLine.History
{
    public class HistoryFileStorageOptions
    {
        public string FilePath { get; set; } = "history";
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
