using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineIinterface.IO
{
    public interface ICommandLineReader
    {
        Task<string> ReadLineAsync(CancellationToken token = default(CancellationToken));
        Task<string> ReadLineAsync(char? mask, CancellationToken token = default(CancellationToken));
    }
}
