using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineInterface.IO
{
    public interface ILineReader
    {
        Task<string> ReadLineAsync(CancellationToken token = default);
        Task<string> ReadLineAsync(char? mask, CancellationToken token = default);
    }
}
