using System.Threading;
using System.Threading.Tasks;

namespace VoidMain.IO.Keyboard
{
    public interface IKeyReader
    {
        ValueTask<KeyInput> ReadKeyAsync(CancellationToken token = default);
    }
}
