using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineIinterface
{
    public interface ICommandLineReader
    {
        Task<string> ReadLineAsync(IPrompt prompt, CancellationToken token);
        Task<string> ReadLineAsync(IPrompt prompt, char? mask, CancellationToken token);
    }
}
