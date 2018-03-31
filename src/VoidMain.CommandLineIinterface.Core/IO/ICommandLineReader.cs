using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineIinterface.IO
{
    public interface ICommandLineReader
    {
        Task<string> ReadLineAsync(CancellationToken token = default(CancellationToken));
        Task<string> ReadLineAsync(char? mask, CancellationToken token = default(CancellationToken));
        Task<string> ReadLineAsync(ICommandLinePrompt prompt, CancellationToken token = default(CancellationToken));
        Task<string> ReadLineAsync(ICommandLinePrompt prompt, char? mask, CancellationToken token = default(CancellationToken));
    }
}
