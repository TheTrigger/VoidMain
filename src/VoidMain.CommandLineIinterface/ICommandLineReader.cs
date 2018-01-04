using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineReader
    {
        Task<string> ReadLineAsync(IPrompt prompt, CancellationToken token = default(CancellationToken));
        Task<string> ReadLineAsync(IPrompt prompt, char? mask, CancellationToken token = default(CancellationToken));
    }
}
