using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineIinterface.Console
{
    public interface ICommandLineReader
    {
        Task<string> ReadLineAsync(IPrompt prompt, CancellationToken token);
    }
}
