using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.CommandLineIinterface.IO.Console
{
    public interface IConsoleKeyReader
    {
        Task<ExtendedConsoleKeyInfo> ReadKeyAsync(
            bool intercept = false, CancellationToken token = default(CancellationToken));
    }
}
